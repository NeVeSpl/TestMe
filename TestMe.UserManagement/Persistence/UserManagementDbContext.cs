using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TestMe.Core.Persistence.UserManagement.Configurations;
using TestMe.UserManagement.Domain;

namespace TestMe.UserManagement.Persistence
{
    public class UserManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.HasDefaultSchema("UserManagement");
            string configurationsPrefix = typeof(UserConfiguration).Namespace;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.FullName.StartsWith(configurationsPrefix));           
        }
    }
}
