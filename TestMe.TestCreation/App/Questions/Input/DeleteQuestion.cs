using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.Questions.Input
{
    public class DeleteQuestion
    {
        public long UserId { get; set; }
        public long QuestionId { get; set; }


        public DeleteQuestion()
        {
        }

        public DeleteQuestion(long userId, long questionId)
        {
            UserId = userId;
            QuestionId = questionId;
        }
    }
}
