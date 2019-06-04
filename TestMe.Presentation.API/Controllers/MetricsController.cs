using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Diagnostics.Tracing;
using System.Runtime;
using TestMe.Presentation.API.Attributes;

namespace TestMe.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : Controller
    {
        private static readonly GcEventListener EventListener = new GcEventListener();
        private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = ".", NumberDecimalDigits = 2 };
        private static readonly Process CurrentProcess = Process.GetCurrentProcess();
        private static long prevMeasuredDateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static double prevMeasuredTotalProcessorTime = CurrentProcess.TotalProcessorTime.TotalMilliseconds;
        

        [HttpGet("lineprotocol")]
        [ResponseCache(Location =ResponseCacheLocation.Any, Duration = 5)]
        [LocalHostOnly]
        public ActionResult LineProtocol(long catalogId)
        {
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var totalProcessorTime = CurrentProcess.TotalProcessorTime.TotalMilliseconds;

            double cpuTimeElapsed = (now - prevMeasuredDateTime) * Environment.ProcessorCount;
            double cpuTimeUsed  = totalProcessorTime - prevMeasuredTotalProcessorTime;
            double cpuUsage = cpuTimeUsed * 100 / cpuTimeElapsed;

            prevMeasuredDateTime = now;
            prevMeasuredTotalProcessorTime = totalProcessorTime;

            var stringBuilder = new StringBuilder();
            
            string cpuValue = cpuUsage.ToString("0.00", NumberFormat);           
            stringBuilder.Append("CPU ");
            stringBuilder.AppendLine(cpuValue);        
       
            stringBuilder.Append("WorkingSet ");
            stringBuilder.AppendLine(Format((ulong)CurrentProcess.WorkingSet64));
            stringBuilder.Append("Gen0Collections ");
            stringBuilder.AppendLine(GC.CollectionCount(0).ToString());
            stringBuilder.Append("Gen1Collections ");
            stringBuilder.AppendLine(GC.CollectionCount(1).ToString());
            stringBuilder.Append("Gen2Collections ");
            stringBuilder.AppendLine(GC.CollectionCount(2).ToString());
            stringBuilder.Append("GCTotalMemory ");     
            stringBuilder.AppendLine(Format((ulong)GC.GetTotalMemory(false)));
            stringBuilder.Append("Gen0HeapSize ");
            stringBuilder.AppendLine(Format(EventListener._gen0Size));
            stringBuilder.Append("Gen1HeapSize ");
            stringBuilder.AppendLine(Format(EventListener._gen1Size));
            stringBuilder.Append("Gen2HeapSize ");
            stringBuilder.AppendLine(Format(EventListener._gen2Size));
            stringBuilder.Append("LOHSize ");
            stringBuilder.AppendLine(Format(EventListener._lohSize));
            stringBuilder.Append("BytesPromotedFromGen0 ");
            stringBuilder.AppendLine(Format(EventListener._gen0Promoted));
            stringBuilder.Append("BytesPromotedFromGen1 ");
            stringBuilder.AppendLine(Format(EventListener._gen1Promoted));
            stringBuilder.Append("BytesSurvivedGen2 ");
            stringBuilder.AppendLine(Format(EventListener._gen2Survived));
            stringBuilder.Append("BytesSurvivedLOH ");
            stringBuilder.AppendLine(Format(EventListener._lohSurvived));
           
            return Content(stringBuilder.ToString());
        }

        private string Format(ulong value)
        {
            return (value / 1024.0f).ToString("0.00", NumberFormat);
        }



        sealed class GcEventListener : EventListener
        {            
            public const int GC_KEYWORD = 0x0000001;   
            public ulong _gen0Size;
            public ulong _gen1Size;
            public ulong _gen2Size;
            public ulong _lohSize;
            public ulong _gen0Promoted;
            public ulong _gen1Promoted;
            public ulong _gen2Survived;
            public ulong _lohSurvived;           

            public GcEventListener()
            {
            }

            protected override void OnEventSourceCreated(EventSource eventSource)
            {               
                if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
                { 
                    EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)GC_KEYWORD);
                }
            }
            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                switch (eventData.EventName)
                {
                    case "GCHeapStats_V1":
                        ProcessHeapStats(eventData);
                        break;                    
                }
            }
            private void ProcessHeapStats(EventWrittenEventArgs eventData)
            {
                _gen0Size = (ulong)eventData.Payload[0];
                _gen0Promoted = (ulong)eventData.Payload[1];
                _gen1Size = (ulong)eventData.Payload[2];
                _gen1Promoted = (ulong)eventData.Payload[3];
                _gen2Size = (ulong)eventData.Payload[4];
                _gen2Survived = (ulong)eventData.Payload[5];
                _lohSize = (ulong)eventData.Payload[6];
                _lohSurvived = (ulong)eventData.Payload[7];
            }
        }
    }
}
