using System;
using System.Collections.Generic;
using System.Text;
using TestMe.TestCreation.App.Tests.Input;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class UpdateQuestionItemDTO : CreateQuestionItemDTO
    {        

        internal UpdateQuestionItem CreateCommand(long userId, long testId, long quetionItemId)
        {
            return new UpdateQuestionItem()
            {
                TestId = testId,
                UserId = userId,
                QuestionItemId = quetionItemId,                
            };
        }
    }
}
