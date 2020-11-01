using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.Tests.CreateTest;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class CreateTestDTO
    { 
        [StringLength(maximumLength: TestConst.TitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        public long OwnerId { get; set; }


        public CreateTestCommand CreateCommand()
        {
            return new CreateTestCommand()
            {                
                Title = Title,           
                OwnerId = OwnerId,
            };
        }
    }
}
