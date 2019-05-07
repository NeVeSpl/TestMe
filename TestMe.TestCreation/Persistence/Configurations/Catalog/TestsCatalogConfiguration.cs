using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class TestsCatalogConfiguration : IEntityTypeConfiguration<TestsCatalog>
    {
        public void Configure(EntityTypeBuilder<TestsCatalog> builder)
        {
            builder.HasBaseType<Catalog>().ToTable("Catalog");          
            builder.Property(x => x.Name).HasMaxLength(Catalog.NameMaxLength);
          
        }
    }
}
