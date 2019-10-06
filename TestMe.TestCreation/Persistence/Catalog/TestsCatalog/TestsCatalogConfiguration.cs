using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class TestsCatalogConfiguration : IEntityTypeConfiguration<TestsCatalog>
    {
        public void Configure(EntityTypeBuilder<TestsCatalog> builder)
        {
            builder.HasBaseType<Catalog>();          
            builder.Property(x => x.Name).HasMaxLength(Catalog.NameMaxLength);
            builder.HasMany(x => x.Tests).WithOne().HasForeignKey(x => x.CatalogId).HasPrincipalKey(x => x.CatalogId);
        }
    }
}
