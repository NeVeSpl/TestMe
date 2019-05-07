using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(x => x.AnswerId);
            builder.Property(x => x.Content).HasMaxLength(Answer.ContentMaxLength);
        }
    }
}
