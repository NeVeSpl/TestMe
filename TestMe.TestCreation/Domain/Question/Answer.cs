using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class Answer
    {
        public const int ContentMaxLength = 2048;


        public long AnswerId
        {
            get;
            private set;
        }
        public bool IsCorrect { get;  set; }
        public short OrdinalNumber { get; private set; }
        public string Content { get;  set; }
        public long QuestionId { get; private set; }
       


        private Answer()
        {

        }

        public static Answer Create(string content, bool isCorrect)
        {
            return new Answer() { Content = content, IsCorrect = isCorrect };
        }
    }
}
