using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly FakeContextDefinition<TestCreationDbContext> testCreationFakeContextDefinition;
        private readonly FakeContextDefinition<UserManagementDbContext> userManagementFakeContextDefinition;
        private readonly Action<IServiceScope> onWebHostReady;
        private IServiceScope serviceScope;
       

        public ApiFactory(DatabaseType databaseType, Action<IServiceScope> onWebHostReady = null, [CallerFilePath]string callerFilePath = "")
        {
            PostgreSQLConfig config = databaseType == DatabaseType.PostgreSQL ? new PostgreSQLConfig(callerFilePath) : null;

            this.onWebHostReady = onWebHostReady;
            testCreationFakeContextDefinition = new FakeContextDefinition<TestCreationDbContext>(databaseType, config);
            userManagementFakeContextDefinition = new FakeContextDefinition<UserManagementDbContext>(databaseType, config);
        }

        public HttpClient CreateClient(string token)
        {
            var httpClient = base.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return httpClient;
        }
        public TContext GetContext<TContext>() where TContext : DbContext
        {
            serviceScope = serviceScope ?? Server.Services.CreateScope();
            TContext context = serviceScope.ServiceProvider.GetRequiredService<TContext>();            
            return context;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        { 
            builder.ConfigureServices(services =>
            {
                // It appears that in .net core 3.0 DbContextOptions provided for DbContext are registered in DI container as singletons behind our backs,
                // in order to use our fake DbContext we first need to remove these singletons                
                RemoveService(services, "DbContextOptions`1"); 

                services.AddDbContextPool<TestCreationDbContext>(options => testCreationFakeContextDefinition.SetOptions(options));
                services.AddDbContextPool<ReadOnlyTestCreationDbContext>(options => testCreationFakeContextDefinition.SetOptions(options));
                services.AddDbContextPool<UserManagementDbContext>(options => userManagementFakeContextDefinition.SetOptions(options));

                var serviceProvider = services.BuildServiceProvider();
               
                using (var scope = serviceProvider.CreateScope())
                {
                    onWebHostReady?.Invoke(scope);
                }
            });          
        }

        private void RemoveService(IServiceCollection services, string name)
        {          
            ServiceDescriptor founded = null;
            do
            {
                founded = services.FirstOrDefault(x => x.ServiceType.Name == name);
                if (founded != null)
                {
                    services.Remove(founded);
                }
            } while (founded != null);
        }

        protected override void Dispose(bool disposing)
        {
            serviceScope?.Dispose();
            testCreationFakeContextDefinition.Dispose();
            userManagementFakeContextDefinition.Dispose();
            base.Dispose(disposing);
        }
        
        private class FakeContextDefinition<TContext>  where TContext  : DbContext
        {
            private readonly DatabaseType databaseType;
            //private readonly InMemoryDatabaseRoot efInMemoryDatabaseRoot;
            private readonly ServiceProvider efServiceProvider;
            private readonly SqliteConnection sqliteConnection;
            private readonly string postgreSQLConnectionString;         


            public FakeContextDefinition(DatabaseType databaseType, PostgreSQLConfig config)
            {
                this.databaseType = databaseType;
               

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
           
           
            public void Dispose()
            {
                sqliteConnection?.Close();
            }

            public void SetOptions(DbContextOptionsBuilder options)
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