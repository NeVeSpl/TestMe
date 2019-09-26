using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    /// <summary>
    /// Migartion to EF 3.0, owned types cannot have separate configuration anymore
    /// https://github.com/aspnet/EntityFrameworkCore/issues/15681    /// 
    /// </summary>
    internal sealed class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasKey(x => x.AnswerId);
            builder.Property(x => x.Content).HasMaxLength(Answer.ContentMaxLength);
        }
    }
}
