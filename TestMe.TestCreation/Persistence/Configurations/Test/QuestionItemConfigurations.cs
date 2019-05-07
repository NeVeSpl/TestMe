using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;


namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class QuestionItemConfigurations : IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.HasKey(x => x.QuestionItemId);
            builder.HasIndex(nameof(QuestionItem.QuestionItemId));
        }
    }
}
