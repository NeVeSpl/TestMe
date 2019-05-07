using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class QuestionsCatalogConfiguration : IEntityTypeConfiguration<QuestionsCatalog>
    {
        public void Configure(EntityTypeBuilder<QuestionsCatalog> builder)
        {
            builder.HasBaseType<Catalog>().ToTable("Catalog");          
            builder.Property(x => x.Name).HasMaxLength(Catalog.NameMaxLength);
            //builder.Property<int>(x => x.QuestionsCount);
        }
    }
}
