using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TestMe.Presentation.API.Attributes;

namespace TestMe.Presentation.API.ControllersSpecial
{
    [ApiController]
    [Route("[controller]")]
    public class MetricsController : Controller
    {
        private static readonly CLREventListener EventListener = new CLREventListener();
        private static readonly NumberFormatInfo NumberFormat = new NumberFormatInfo() { NumberDecimalSeparator = ".", NumberDecimalDigits = 2 };
        private static readonly Process CurrentProcess = Process.GetCurrentProcess();
        private static long previouslyMeasuredDateTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        private static double previouslyMeasuredTotalProcessorTime = CurrentProcess.TotalProcessorTime.TotalMilliseconds;


        [HttpGet("lineprotocol")]
        [ResponseCache(Location =ResponseCacheLocation.Any, Duration = 5)]
        [LocalHostOnly]
        public ActionResult LineProtocol(long catalogId)
        {
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var totalProcessorTime = CurrentProcess.TotalProcessorTime.TotalMilliseconds;

            double cpuTimeElapsed = (now - previouslyMeasuredDateTime) * Environment.ProcessorCount;
            double cpuTimeUsed  = totalProcessorTime - previouslyMeasuredTotalProcessorTime;
            double cpuUsage = cpuTimeUsed * 100 / cpuTimeElapsed;

            previouslyMeasuredDateTime = now;
            previouslyMeasuredTotalProcessorTime = totalProcessorTime;

            var result = new StringBuilder();            
                   
            result.Append("CPU ");
            result.AppendLine(cpuUsage.ToString("0.00", NumberFormat));  
            result.Append("WorkingSet ");
            result.AppendLine(ConvertBytesToKBs((ulong)CurrentProcess.WorkingSet64));
            result.Append("Gen0Collections ");
            result.AppendLine(GC.CollectionCount(0).ToString());
            result.Append("Gen1Collections ");
            result.AppendLine(GC.CollectionCount(1).ToString());
            result.Append("Gen2Collections ");
            result.AppendLine(GC.CollectionCount(2).ToString());
            result.Append("GCTotalMemory ");     
            result.AppendLine(ConvertBytesToKBs((ulong)GC.GetTotalMemory(false)));
            result.Append("Gen0HeapSize ");
            result.AppendLine(ConvertBytesToKBs(EventListener.GenerationSize0));
            result.Append("Gen1HeapSize ");
            result.AppendLine(ConvertBytesToKBs(EventListener.GenerationSize1));
            result.Append("Gen2HeapSize ");
            result.AppendLine(ConvertBytesToKBs(EventListener.GenerationSize2));
            result.Append("LOHSize ");
            result.AppendLine(ConvertBytesToKBs(EventListener.GenerationSize3));
            result.Append("BytesPromotedFromGen0 ");
            result.AppendLine(ConvertBytesToKBs(EventListener.TotalPromotedSize0));
            result.Append("BytesPromotedFromGen1 ");
            result.AppendLine(ConvertBytesToKBs(EventListener.TotalPromotedSize1));
            result.Append("BytesSurvivedGen2 ");
            result.AppendLine(ConvertBytesToKBs(EventListener.TotalPromotedSize2));
            result.Append("BytesSurvivedLOH ");
            result.AppendLine(ConvertBytesToKBs(EventListener.TotalPromotedSize3));
            result.Append("ExceptionCount ");
            result.AppendLine(EventListener.ExceptionCount.ToString());
            result.Append("GCPause ");
            result.AppendLine(EventListener.GCPause.ElapsedMilliseconds.ToString("0.00", NumberFormat));

            return Content(result.ToString());
        }

        private string ConvertBytesToKBs(ulong value)
        {
            return (value / 1024.0f).ToString("0.00", NumberFormat);
        }



        sealed class CLREventListener : EventListener
        {            
            private const int GC_KEYWORD = 0x1;
            private const int ExceptionKeyword = 0x8000;
            public readonly Stopwatch GCPause = new Stopwatch();
            public ulong GenerationSize0;
            public ulong GenerationSize1;
            public ulong GenerationSize2;
            public ulong GenerationSize3;
            public ulong TotalPromotedSize0;
            public ulong TotalPromotedSize1;
            public ulong TotalPromotedSize2;
            public ulong TotalPromotedSize3;
            public ulong ExceptionCount;

           

            protected override void OnEventSourceCreated(EventSource eventSource)
            {               
                if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
                { 
                    EnableEvents(eventSource, EventLevel.Informational, (EventKeywords)(GC_KEYWORD | ExceptionKeyword));  
                }
            }
            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                switch (eventData.EventName)
                {
                    case "GCHeapStats_V1":
                        ProcessHeapStats(eventData);
                        break;
                    case "ExceptionThrown_V1":
                        string exception = eventData.Payload[0] as string;
                        if (exception == "TestMe.SharedKernel.Domain.DomainException") return;
                        ExceptionCount++;
                        break;
                    case "GCSuspendEEEnd_V1":
                        GCPause.Start();
                        break;
                    case "GCRestartEEEnd_V1":
                        GCPause.Stop();
                        break;
                }
            }
            private void ProcessHeapStats(EventWrittenEventArgs eventData)
            {
                GenerationSize0 = (ulong)eventData.Payload[0];
                TotalPromotedSize0 = (ulong)eventData.Payload[1];
                GenerationSize1 = (ulong)eventData.Payload[2];
                TotalPromotedSize1 = (ulong)eventData.Payload[3];
                GenerationSize2 = (ulong)eventData.Payload[4];
                TotalPromotedSize2 = (ulong)eventData.Payload[5];
                GenerationSize3 = (ulong)eventData.Payload[6];
                TotalPromotedSize3 = (ulong)eventData.Payload[7];
            }
        }
    }
}
