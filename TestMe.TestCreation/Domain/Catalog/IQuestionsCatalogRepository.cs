using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal interface IQuestionsCatalogRepository
    {       
        QuestionsCatalog GetById(long id, bool includeQuestions = false);     
    }
}
