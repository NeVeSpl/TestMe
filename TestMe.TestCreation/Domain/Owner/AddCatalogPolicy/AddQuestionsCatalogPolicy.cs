using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class AddQuestionsCatalogPolicy : IAddQuestionsCatalogPolicy
    {
        private readonly long maxNumberOfQuestionsCatalogs;


        public AddQuestionsCatalogPolicy(long maxNumberOfQuestionsCatalogs)
        {
            this.maxNumberOfQuestionsCatalogs = maxNumberOfQuestionsCatalogs;
        }
        

        public bool CanAddQuestionsCatalog(long questionsCount)
        {
            return questionsCount < maxNumberOfQuestionsCatalogs;
        }
    }
}
