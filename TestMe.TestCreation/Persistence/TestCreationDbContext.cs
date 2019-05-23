using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal class TestCreationDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; protected set; }

        public DbSet<QuestionsCatalog> QuestionsCatalogs { get; protected set; }
        public DbSet<TestsCatalog> TestsCatalogs { get; protected set; }

        public DbSet<Question> Questions { get; protected set; }
        public DbSet<Answer> Answers { get; protected set; }        

        public DbSet<Test> Tests { get; protected set; }
        public DbSet<QuestionsCatalogItem> QuestionsCatalogItems { get; protected set; }
        public DbSet<QuestionItem> QuestionItems { get; protected set; }


        public TestCreationDbContext(DbContextOptions options) : base(options)
        {
                  
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.HasDefaultSchema("TestCreation");
            string configurationsPrefix = typeof(Configurations.CatalogConfiguration).Namespace;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.FullName.StartsWith(configurationsPrefix));         
        }
    }

    internal class ReadOnlyTestCreationDbContext : TestCreationDbContext
    {

        public ReadOnlyTestCreationDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
    }
}