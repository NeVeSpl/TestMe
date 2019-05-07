using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class CatalogConfiguration : IEntityTypeConfiguration<Catalog>
    {
        public void Configure(EntityTypeBuilder<Catalog> builder)
        {
            builder.HasQueryFilter(p => !p.IsDeleted);
            builder.HasIndex(nameof(Catalog.CatalogId));
        }
    }
}
