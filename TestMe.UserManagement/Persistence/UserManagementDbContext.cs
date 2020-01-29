using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.EventBus;
using TestMe.UserManagement.Domain;
using TestMe.UserManagement.Persistence.Configurations;

namespace TestMe.UserManagement.Persistence
{
    public sealed class UserManagementDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; }
        public DbSet<Event> Outbox { get; private set; }


#pragma warning disable CS8618 // Non-nullable field is uninitialized
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {

        }
#pragma warning restore CS8618 



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.HasDefaultSchema("UserManagement");
            string configurationsPrefix = typeof(UserConfiguration).Namespace!;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.FullName?.StartsWith(configurationsPrefix) ?? false);           
        }
    }
}
