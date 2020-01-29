using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.Tests;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.Tests
{
    public abstract class BaseFixture
    {        
        private protected FakeContextDefinition<TestCreationDbContext> fakeContextDefinition;
        private protected abstract FakeDatabaseType GetDatabaseType();


        [TestInitialize()]
        public void Startup()
        {
            fakeContextDefinition = new FakeContextDefinition<TestCreationDbContext>(GetDatabaseType());
            using (var context = new TestCreationDbContext(fakeContextDefinition))
            {                
                context.Database.EnsureCreated();
                TestUtils.Seed(context);
            }
        }
        [TestCleanup()]
        public void Cleanup()
        {
            fakeContextDefinition.Cleanup();          
        }

        private protected TestCreationDbContext CreateTestCreationDbContext()
        {
            return new TestCreationDbContext(fakeContextDefinition);
        }
        private protected ReadOnlyTestCreationDbContext CreateReadOnlyTestCreationDbContext()
        {
            return new ReadOnlyTestCreationDbContext(fakeContextDefinition);
        }        
    }
}