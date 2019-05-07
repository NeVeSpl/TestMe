using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal interface IQuestionRepository
    {
        Question GetByIdWithAnswers(long id);       
    }
}
