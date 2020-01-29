using System;
using System.Collections.Generic;
using System.Text;

namespace TestMe.TestCreation.App.Tests.Input
{
    public class DeleteQuestionItem
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public long QuestionItemId { get; set; }


        public DeleteQuestionItem()
        {
        }

        public DeleteQuestionItem(long userId, long testId, long questionItemId)
        {
            UserId = userId;
            TestId = testId;
            QuestionItemId = questionItemId;
        }        
    }
}