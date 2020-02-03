using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TestMe.TestCreation.Persistence
{
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