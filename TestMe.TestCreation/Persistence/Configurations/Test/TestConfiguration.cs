using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestMe.TestCreation.Domain;


namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.OwnsMany(x => x.Questions, questionsBuilder => 
            {
                questionsBuilder.HasKey(x => x.QuestionItemId);
                questionsBuilder.HasIndex(nameof(QuestionItem.QuestionItemId));
            }
            );
            builder.HasQueryFilter(p => !p.IsDeleted);
            builder.Property(x => x.Title).HasMaxLength(Test.TitleMaxLength);
            builder.HasIndex(nameof(Test.TestId));
        }
    }
}
