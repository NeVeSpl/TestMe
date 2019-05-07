using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.Persistence;

namespace TestMe.Presentation.API.Tests.Utils
{ 
    internal sealed class ApiFactory : WebApplicationFactory<Startup>
    {
        public enum DatabaseType{ PostgreSQL, EFInMemory, SQLiteInMemory}

        private readonly List<IFakeContextDefinition<DbContext>> fakeContextsDefinitions;
        private IServiceScope serviceScope;
       

        public ApiFactory(DatabaseType databaseType, [CallerFilePath]string callerFilePath = "")
        {
            PostgreSQLConfig config = databaseType == DatabaseType.PostgreSQL ? new PostgreSQLConfig(callerFilePath) : null;

            fakeContextsDefinitions = new List<IFakeContextDefinition<DbContext>>()
            {
                new FakeContextDefinition<TestCreationDbContext>(databaseType, config, TestCreation.TestUtils.Seed),
                new FakeContextDefinition<UserManagementDbContext>(databaseType, config, UserManagement.Persistence.TestUtils.Seed)
            };
            
        }

        public HttpClient CreateClient(string token)
        {
            var httpClient = base.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return httpClient;
        }

        public TContext GetContext<TContext>() where TContext : DbContext
        {
            serviceScope = serviceScope ?? Server.Host.Services.CreateScope();
            TContext context = serviceScope.ServiceProvider.GetRequiredService<TContext>();
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {          
            builder.ConfigureServices(services =>
            {              
                foreach (var fakeContextDefinition in fakeContextsDefinitions)
                {
                    fakeContextDefinition.SetupServices(services);                   
                }

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var serviceScope = serviceProvider.CreateScope();

                    foreach (var fakeContextDefinition in fakeContextsDefinitions)
                    {
                        fakeContextDefinition.Seed(serviceScope.ServiceProvider);
                    }
                }        
            });          
        }

        protected override void Dispose(bool disposing)
        {
            serviceScope?.Dispose();
            foreach (var fakeContextDefinition in fakeContextsDefinitions)
            {
                fakeContextDefinition.Dispose();
            }
            base.Dispose(disposing);
        }


        private class FakeContextDefinition<TContext> : IFakeContextDefinition<TContext> where TContext : DbContext
        {
            private readonly DatabaseType databaseType;
            //private readonly InMemoryDatabaseRoot efInMemoryDatabaseRoot;
            private readonly ServiceProvider efServiceProvider;
            private readonly SqliteConnection sqliteConnection;
            private readonly string postgreSQLConnectionString;
            private readonly Action<TContext> seed;


            public FakeContextDefinition(DatabaseType databaseType, PostgreSQLConfig config, Action<TContext> seed)
            {
                this.databaseType = databaseType;
                this.seed = seed;

                switch (databaseType)
                {
                    case DatabaseType.PostgreSQL:                       
                        postgreSQLConnectionString = config.GenerateConnectionString(typeof(TContext).Name);
                        break;
                    case DatabaseType.EFInMemory:
                        /* There are two ways to create distinct ef in memory databases: by using InMemoryDatabaseRoot or ServiceProvider
                         */
                        //efInMemoryDatabaseRoot = new InMemoryDatabaseRoot();                   
                        efServiceProvider = new ServiceCollection()
                            .AddEntityFrameworkInMemoryDatabase()
                            .BuildServiceProvider();                       
                        break;
                    case DatabaseType.SQLiteInMemory:                        
                        sqliteConnection = new SqliteConnection("DataSource=:memory:");
                        sqliteConnection.Open();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }            

            public void SetupServices(IServiceCollection services)
            {
                services.AddDbContextPool<TContext>(options => SetOptions(options));
            }
            public void Seed(IServiceProvider serviceProvider)
            {               
                var context = serviceProvider.GetRequiredService<TContext>();               
                context.Database.EnsureDeleted();               
                context.Database.EnsureCreated();
               
                seed(context);              
            }
            public void Dispose()
            {
                sqliteConnection?.Close();
            }

            private void SetOptions(DbContextOptionsBuilder options)
            {
                switch (databaseType)
                {
                    case DatabaseType.PostgreSQL:
                        options.UseNpgsql(postgreSQLConnectionString);
                        break;
                    case DatabaseType.EFInMemory:                      
                        options.UseInMemoryDatabase("InMemoryDatabase");
                        options.UseInternalServiceProvider(efServiceProvider);
                        break;
                    case DatabaseType.SQLiteInMemory:
                        options.UseSqlite(sqliteConnection);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }            
        }
        private interface IFakeContextDefinition<out TContext> where TContext : DbContext
        {
            void SetupServices(IServiceCollection services);
            void Seed(IServiceProvider serviceProvider);
            void Dispose();
        }

        private class PostgreSQLConfig
        {
            uint TestId { get;  }
            string ConnectionStringTemplate { get; }
            string TestFileName { get; }

            public PostgreSQLConfig(string testFilePath)
            {
                TestFileName = Path.GetFileName(testFilePath);
                TestId = GetDeterministicHashCode(testFilePath);
                ConnectionStringTemplate = GetPostgreSQLConnectionStringTemplate();
            }

            public string GenerateConnectionString(string contextName)
            {
                return String.Format(ConnectionStringTemplate, $"{TestFileName}_{TestId}_{contextName}");
            }

            private uint GetDeterministicHashCode(string str)
            {
                unchecked
                {
                    uint hash1 = (5381 << 16) + 5381;
                    uint hash2 = hash1;

                    for (int i = 0; i < str.Length; i += 2)
                    {
                        hash1 = ((hash1 << 5) + hash1) ^ str[i];
                        if (i == str.Length - 1)
                            break;
                        hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                    }

                    return hash1 + (hash2 * 1566083941);
                }
            }

            private string GetPostgreSQLConnectionStringTemplate()
            {
                //var currentDirectory = Directory.GetCurrentDirectory();

                //var configurationBuilder = new ConfigurationBuilder()
                //  .SetBasePath(currentDirectory)
                //  .AddJsonFile("testsettings.json");

                //var configuration = configurationBuilder.Build();
                //var connectionString = configuration.GetConnectionString("PostgreSQL");

                //return connectionString;
                return "Host=localhost;Database=ApiTest_{0};Username=TestDBUser;Password=test;";
            }
        }
    }
}
