using Microsoft.EntityFrameworkCore.Design;
using TestMe.BuildingBlocks.Persistence;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TestCreationDbContext>
    {
        public TestCreationDbContext CreateDbContext(string[] args)
        { 
            return new TestCreationDbContext(DbContextOptionsFactory.Create<TestCreationDbContext>());
        }
    }
}