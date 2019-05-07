using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.Persistence.Configurations
{
    internal sealed class QuestionsCatalogItemConfigurations : IEntityTypeConfiguration<QuestionsCatalogItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuestionsCatalogItem> builder)
        {
            builder.HasKey(x => x.CatalogOfQuestionsItemId);
        }
    }
}
