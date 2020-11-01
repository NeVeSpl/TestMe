using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace TestMe.BuildingBlocks.Tests
{
    public class FakeContextDefinition<TContext> where TContext : DbContext
    {
        private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        private readonly FakeDatabaseType databaseType;
        private readonly DbContextOptions<TContext> options;
        private SqliteConnection? sqliteConnection;
        private InMemoryDatabaseRoot? efInMemoryDatabaseRoot;
        //private ServiceProvider efServiceProvider;

        public FakeContextDefinition(FakeDatabaseType databaseType)
        {
            this.databaseType = databaseType;
            var builder = new DbContextOptionsBuilder<TContext>();
            SetupBuilder(builder);
            options = builder.Options;
            
        }

        public void SetupBuilder(DbContextOptionsBuilder builder)
        {
            switch (databaseType)
            {
                case FakeDatabaseType.EFInMemory:
                    /* There are two ways to create distinct ef in-memory databases: by using InMemoryDatabaseRoot or ServiceProvider */
                    efInMemoryDatabaseRoot ??= new InMemoryDatabaseRoot();
                    builder.UseInMemoryDatabase(databaseName: "DataSource", efInMemoryDatabaseRoot);
                    //efServiceProvider ??= new ServiceCollection()
                    //       .AddEntityFrameworkInMemoryDatabase()
                    //       .BuildServiceProvider();
                    //builder.UseInMemoryDatabase("InMemoryDatabase");
                    //builder.UseInternalServiceProvider(efServiceProvider);
                    break;
                case FakeDatabaseType.SQLiteInMemory:
                    sqliteConnection ??= new SqliteConnection("DataSource=:memory:");
                    sqliteConnection.Open();
                    builder.UseSqlite(sqliteConnection);
                    break;
                case FakeDatabaseType.PostgreSQL:
                    string contextName = typeof(TContext).Name;
                    builder.UseNpgsql($"Host=localhost;Database=Test_{contextName};Username=TestMeDBUser;Password=TestMePass;");
                    break;
                default:
                    throw new NotImplementedException();
            }
            builder.UseLoggerFactory(LoggerFactory);
            builder.EnableDetailedErrors();
            builder.EnableSensitiveDataLogging();
        }


        public void Cleanup()
        {
            sqliteConnection?.Close();
        }


        public static implicit operator DbContextOptions<TContext>(FakeContextDefinition<TContext> fakeContextDefinition)
        {
            return fakeContextDefinition.options;
        }
    }
}