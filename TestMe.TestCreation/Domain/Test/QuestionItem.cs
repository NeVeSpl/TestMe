using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class QuestionItem 
    {
        public long QuestionItemId { get; private set; }

        public Question Question { get; private set; }
        public long TestId { get; private set; }
        
        
        private QuestionItem()
        {

        }

        public static QuestionItem Create(Question question)
        {
            return new QuestionItem() { Question = question };
        }
    }
}
