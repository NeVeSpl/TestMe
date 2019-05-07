using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal interface IAddQuestionPolicy
    {
        bool CanAddQuestion(int questionsCount);
    }
}
