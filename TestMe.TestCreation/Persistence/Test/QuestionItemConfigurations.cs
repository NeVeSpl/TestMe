using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;


namespace TestMe.TestCreation.Persistence
{
    /// <summary>
    /// Migartion to EF 3.0, owned types cannot have separate configuration anymore
    /// https://github.com/aspnet/EntityFrameworkCore/issues/15681    /// 
    /// </summary>
    internal sealed class QuestionItemConfigurations : IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.HasKey(x => x.QuestionItemId);
            builder.HasIndex(nameof(QuestionItem.QuestionItemId));
        }
    }
}
