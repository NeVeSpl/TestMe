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


#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private QuestionsCatalogItem()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {

        }
    }
}
