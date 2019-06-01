using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestMe.Presentation.React
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .ConfigureKestrel(x =>
                    {
                        x.AllowSynchronousIO = false;
                        x.AddServerHeader = false;
                        x.Limits.MaxConcurrentConnections = 1000;
                        x.Limits.MaxConcurrentUpgradedConnections = 1000;
                        x.Limits.MaxRequestBodySize = 5_000_000;
                    })
                .UseStartup<Startup>();
    }
}
