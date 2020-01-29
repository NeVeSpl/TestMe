using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence
{
    internal sealed class QuestionsCatalogItemConfigurations : IEntityTypeConfiguration<QuestionsCatalogItem>
    {
        public void Configure(EntityTypeBuilder<QuestionsCatalogItem> builder)
        {
            builder.HasKey(x => x.CatalogOfQuestionsItemId);
        }
    }
}
