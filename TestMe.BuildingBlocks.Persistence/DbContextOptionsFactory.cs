using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TestMe.BuildingBlocks.Persistence
{
    public static class DbContextOptionsFactory
    {
        public static DbContextOptions<TContext> Create<TContext>() where TContext : DbContext
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../TestMe.Presentation.API/");

            var configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(currentDirectory)
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{environmentName}.json", true);

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString(typeof(TContext).Name);

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return optionsBuilder.Options;
        }
    }
}