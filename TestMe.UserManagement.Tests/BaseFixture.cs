using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMe.BuildingBlocks.Tests;
using TestMe.UserManagement;
using TestMe.UserManagement.Infrastructure.AutoMapper;
using TestMe.UserManagement.Persistence;

namespace TestMe.UserManagement.Tests
{
    public abstract class BaseFixture
    {        
        private protected FakeContextDefinition<UserManagementDbContext> fakeContextDefinition;
        private protected abstract FakeDatabaseType GetDatabaseType();


        [TestInitialize()]
        public void Startup()
        {
            fakeContextDefinition = new FakeContextDefinition<UserManagementDbContext>(GetDatabaseType());
            using (var context = new UserManagementDbContext(fakeContextDefinition))
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

        private protected UserManagementDbContext CreateUserManagementDbContext()
        {
            return new UserManagementDbContext(fakeContextDefinition);
        }
        private protected MapperConfiguration CreateAutoMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserProfile());
            });
            mappingConfig.AssertConfigurationIsValid();
            return mappingConfig;
        }
    }
}