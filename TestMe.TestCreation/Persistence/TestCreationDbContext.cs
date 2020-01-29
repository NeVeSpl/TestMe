using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TestMe.BuildingBlocks.EventBus;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal class TestCreationDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; protected set; }

        public DbSet<QuestionsCatalog> QuestionsCatalogs { get; protected set; }
        public DbSet<TestsCatalog> TestsCatalogs { get; protected set; }

        public DbSet<Question> Questions { get; protected set; }
               

        public DbSet<Test> Tests { get; protected set; }
        
        public DbSet<Event> Inbox { get; protected set; }


#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public TestCreationDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            //modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.HasDefaultSchema("TestCreation");
            string? configurationsPrefix = typeof(TestCreationDbContext).Namespace;
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(), x => x.FullName?.StartsWith(configurationsPrefix!) ?? false);         
        }
    }

    internal sealed class ReadOnlyTestCreationDbContext : TestCreationDbContext
    {
        /* Migration to EF core 3.0 - owned entites cannot be longer used also as Query Type/Keyless Entity Types
        public DbSet<Answer> Answers
        {
            get; protected set;
        }
        public DbSet<QuestionsCatalogItem> QuestionsCatalogItems { get; protected set; }
        public DbSet<QuestionItem> QuestionItems { get; protected set; }
        */


        public ReadOnlyTestCreationDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }   
}