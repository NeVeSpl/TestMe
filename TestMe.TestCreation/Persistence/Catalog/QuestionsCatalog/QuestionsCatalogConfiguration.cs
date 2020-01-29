using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class QuestionsCatalogConfiguration : IEntityTypeConfiguration<QuestionsCatalog>
    {
        public void Configure(EntityTypeBuilder<QuestionsCatalog> builder)
        {
            builder.HasBaseType<Catalog>();          
            builder.Property(x => x.Name).HasMaxLength(CatalogConst.NameMaxLength);
            builder.HasMany(x => x.Questions).WithOne().HasForeignKey(x => x.CatalogId).HasPrincipalKey(x => x.CatalogId);
            //builder.Property<int>(x => x.QuestionsCount);
        }
    }
}
