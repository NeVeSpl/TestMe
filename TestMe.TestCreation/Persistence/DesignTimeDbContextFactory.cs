using Microsoft.EntityFrameworkCore.Design;
using TestMe.SharedKernel.Persistence;

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