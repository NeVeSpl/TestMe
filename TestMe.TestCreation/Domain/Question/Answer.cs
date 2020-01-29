using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.Domain
{
    internal sealed class Answer
    {
        public long AnswerId
        {
            get;
            private set;
        }
        public bool IsCorrect { get;  set; }
        public short OrdinalNumber { get; private set; }
        public string Content { get;  set; }
        public long QuestionId { get; private set; }
       


        private Answer(string content)
        {
            Content = content;
        }

        public static Answer Create(string content, bool isCorrect)
        {
            return new Answer(content) { IsCorrect = isCorrect };
        }
    }
}
