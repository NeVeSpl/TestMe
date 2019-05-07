using Microsoft.EntityFrameworkCore.Design;
using TestMe.SharedKernel.Persistence;

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