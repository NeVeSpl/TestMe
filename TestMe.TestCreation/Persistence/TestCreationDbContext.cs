using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal class TestCreationDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; private set; }

        public DbSet<QuestionsCatalog> QuestionsCatalogs { get; private set; }
        public DbSet<TestsCatalog> TestsCatalogs { get; private set; }

        public DbSet<Question> Questions { get; private set; }
        public DbSet<Answer> Answers { get; private set; }        

        public DbSet<Test> Tests { get; private set; }
        public DbSet<QuestionsCatalogItem> QuestionsCatalogItems { get; private set; }
        public DbSet<QuestionItem> QuestionItems { get; private set; }


        public TestCreationDbContext(DbContextOptions<TestCreationDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.HasDefaultSchema("TestCreation");
            string configurationsPrefix = typeof(Configurations.CatalogConfiguration).Namespace;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.FullName.StartsWith(configurationsPrefix));         
        }
    }
}