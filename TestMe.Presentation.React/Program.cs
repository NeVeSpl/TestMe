using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestMe.Presentation.React
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)            
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureKestrel(x =>
                    {
                        x.AllowSynchronousIO = false;
                        x.AddServerHeader = false;
                        x.Limits.MaxConcurrentConnections = 1000;
                        x.Limits.MaxConcurrentUpgradedConnections = 1000;
                        x.Limits.MaxRequestBodySize = 5_000_000;
                    })
                    .UseStartup<Startup>();
                });
    }
}
