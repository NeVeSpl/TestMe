using System;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.Persistence.Extensions;

namespace TestMe.Presentation.API
{
    internal sealed class Program
    {
        public static int Main(string[] args)
        {
            // https://devblogs.microsoft.com/aspnet/improvements-in-net-core-3-0-for-troubleshooting-and-monitoring-distributed-apps/
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var currentDirectory = Directory.GetCurrentDirectory();
            IConfiguration configuration = CreateConfiguration(currentDirectory);
            Log.Logger = CreateSerilogger(configuration, currentDirectory);

            try
            {
                Log.Information("Start web host");

                /* Everything what is done here it is only applied to application that runs outside of automated tests   
                 * it is why UseSerilog() is invoked here instead within CreateHostBuilder()
                 * it is why Migrate...() is invoked here instead within Startup.Configure()
                 * we do not want to invoke them when we are testing app
                  */
                var hostBuilder = CreateHostBuilder(args).UseSerilog();
                var webHost = hostBuilder.Build();
                webHost.Services.MigrateTestCreationDb();
                webHost.Services.MigrateUserManagementDb();

                Log.Information("Run web host");

                webHost.Run();

                Log.Information("End web host");

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Web host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder =>
                       {
                           webBuilder.ConfigureKestrel(serverOptions =>
                           {
                               serverOptions.AllowSynchronousIO = true;
                               serverOptions.AddServerHeader = false;                              
                           })
                           .UseStartup<Startup>();
                       });                         
        }        

        private static IConfiguration CreateConfiguration(string path)
        {           
            var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{currentEnv}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }        

        private static Logger CreateSerilogger(IConfiguration configuration, string path)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)            
                .Enrich.FromLogContext()
                .WriteTo.File
                (
                    path: Path.Combine(path, @".log"),
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {RequestId} {SourceContext} {EventId} {Message:lj}{NewLine}{Exception}{Scope}",                   
                    rollingInterval: RollingInterval.Day
                ).WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat= "presentation-api-{0:yyyy.MM.dd}"
                })
                .CreateLogger();
        }
    }
}
