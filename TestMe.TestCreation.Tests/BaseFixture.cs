using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.Tests
{
    public abstract class BaseFixture
    {
        private protected enum DatabaseType { EFInMemory, SQLiteInMemory }

        private static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });
        private FakeContextDefinition<TestCreationDbContext> fakeContextDefinition;            
        private DatabaseType databaseType;


        [TestInitialize()]
        public void Startup()
        {
            databaseType = GetDatabaseType();           
        }
        [TestCleanup()]
        public void Cleanup()
        {
            fakeContextDefinition?.sqliteConnection?.Close();           
        }


        private protected TestCreationDbContext CreateTestCreationDbContext()
        {
            bool isFirstTime = fakeContextDefinition == null;
            fakeContextDefinition = fakeContextDefinition ??  new FakeContextDefinition<TestCreationDbContext>(databaseType);           
            var context = new TestCreationDbContext(fakeContextDefinition.options);
            if (isFirstTime)
            {
                context.Database.EnsureCreated();
            }
            return context;
        }
        

        private protected abstract DatabaseType GetDatabaseType();
        

        private class FakeContextDefinition<TContext> where TContext : DbContext
        {
            public readonly SqliteConnection sqliteConnection;
            public readonly DbContextOptions<TContext> options;


            public FakeContextDefinition(DatabaseType databaseType)
            {
                var builder = new DbContextOptionsBuilder<TContext>();

                switch (databaseType)
                {
                    case DatabaseType.EFInMemory:
                        InMemoryDatabaseRoot efInMemoryDatabaseRoot = new InMemoryDatabaseRoot();
                        builder.UseInMemoryDatabase(databaseName: "DataSource", efInMemoryDatabaseRoot);                       
                        break;
                    case DatabaseType.SQLiteInMemory:
                        sqliteConnection = new SqliteConnection("DataSource=:memory:");
                        sqliteConnection.Open();
                        builder.UseSqlite(sqliteConnection);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                builder.UseLoggerFactory(LoggerFactory); 
                builder.EnableSensitiveDataLogging();
                options = builder.Options;                
            }
        }
    }
}
