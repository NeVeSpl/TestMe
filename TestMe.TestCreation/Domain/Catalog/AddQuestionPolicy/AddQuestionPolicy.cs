using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class AddQuestionPolicy : IAddQuestionPolicy
    {
        private readonly int maxNumberOfQuestions;


        public AddQuestionPolicy(int maxNumberOfQuestions)
        {
            this.maxNumberOfQuestions = maxNumberOfQuestions;
        }
        

        public bool CanAddQuestion(int questionsCount)
        {
            return questionsCount < maxNumberOfQuestions;
        }
    }
}
