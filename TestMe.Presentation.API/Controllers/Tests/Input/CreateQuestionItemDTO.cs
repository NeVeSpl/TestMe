using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Tests.Input;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class CreateQuestionItemDTO
    {
        [Required]
        public long? QuestionId { get; set; }

        public CreateQuestionItem CreateCommand(long userId, long testId)
        {
            return new CreateQuestionItem()
            {
                TestId = testId,
                QuestionId = QuestionId!.Value,
                UserId = userId
            };
        }
    }
}
