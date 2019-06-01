using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using TestMe.TestCreation.Persistence;

namespace TestMe.Presentation.API
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            IConfiguration configuration = CreateConfiguration(currentDirectory);
            CreateSerilogger(configuration, currentDirectory);

            try
            {
                Log.Information("Start web host");

                /* Everything what is done here it is only applied to application that runs outside of automatic tests   
                 * it is why UseSerilog() is invoked here instead within CreateWebHostBuilder()
                 * it is why Migrate...() is invoked here instead within Startup.Configure()
                 * we do not want to invoke them when we are testing app
                  */
                IWebHost webHost = CreateWebHostBuilder(args).UseSerilog().Build();
                webHost.Services.MigrateTestCreationDb();
                webHost.Services.MigrateUserManagementDb();
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

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureKestrel(x =>
                          {
                              x.AllowSynchronousIO = true;
                              x.AddServerHeader = false;
                          })
                          .UseStartup<Startup>();
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

        private static void CreateSerilogger(IConfiguration configuration, string path)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)            
                .Enrich.FromLogContext()
                .WriteTo.File
                (
                    path: Path.Combine(path, @".log"),
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {RequestId} {SourceContext} {EventId}{NewLine}               {Message:lj}{NewLine}{Exception}",                   
                    rollingInterval: RollingInterval.Day
                )

            .CreateLogger();
        }
    }
}
