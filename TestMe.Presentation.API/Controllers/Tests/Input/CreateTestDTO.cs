using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Tests.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Tests.Input
{
    public class CreateTestDTO
    {
        [Required]
        public long? CatalogId { get; set; }

        [StringLength(maximumLength: TestConst.TitleMaxLength)]
        public string Title { get; set; } = string.Empty;


        public CreateTest CreateCommand(long userId)
        {
            return new CreateTest()
            {
                CatalogId = CatalogId!.Value,
                Title = Title,
                UserId = userId
            };
        }
    }
}
