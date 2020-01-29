using Microsoft.EntityFrameworkCore.Design;
using TestMe.BuildingBlocks.Persistence;

namespace TestMe.UserManagement.Persistence
{
    internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserManagementDbContext>
    {
        public UserManagementDbContext CreateDbContext(string[] args)
        {
            return new UserManagementDbContext(DbContextOptionsFactory.Create<UserManagementDbContext>());
        }
    }
}