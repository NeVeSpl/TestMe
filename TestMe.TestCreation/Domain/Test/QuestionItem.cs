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


#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        private QuestionItem()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {

        }

        public static QuestionItem Create(Question question)
        {
            return new QuestionItem() { Question = question };
        }
    }
}
