using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(x => x.Content).HasMaxLength(Question.ContentMaxLength);
            builder.OwnsMany(x => x.Answers, 
                answerBuilder => 
                {
                    answerBuilder.HasKey(x => x.AnswerId);
                    answerBuilder.Property(x => x.Content).HasMaxLength(Answer.ContentMaxLength);
                });
            builder.HasQueryFilter(p => !p.IsDeleted);

            builder.HasIndex(nameof(Question.QuestionId));

            //builder.ForNpgsqlUseXminAsConcurrencyToken();
            //builder.Property("xmin").HasDefaultValue(0);

            builder
                .Property(e => e.ConcurrencyToken)
                .HasDefaultValue(0)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            
        }
    }
}
