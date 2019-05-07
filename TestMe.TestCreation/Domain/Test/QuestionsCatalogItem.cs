using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class QuestionsCatalogItem
    {
        public long CatalogOfQuestionsItemId { get; private set; }

        public QuestionsCatalog CatalogOfQuestions { get; private set; }
        public long TestId { get; private set; }


        private QuestionsCatalogItem()
        {

        }
    }
}
