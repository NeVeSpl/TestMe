using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.Tests.CreateTestItem;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class CreateTestItemDTO
    {
        [Required]
        public long? QuestionId { get; set; }

        public CreateTestItemCommand CreateCommand(long testId)
        {
            return new CreateTestItemCommand(testId, QuestionId!.Value);
            
        }
    }
}