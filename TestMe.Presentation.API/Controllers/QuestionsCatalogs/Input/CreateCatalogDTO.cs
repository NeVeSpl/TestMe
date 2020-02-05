using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.QuestionsCatalogs.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input
{
    public class CreateCatalogDTO
    {
        [Required]
        [StringLength(maximumLength: CatalogConst.NameMaxLength, MinimumLength = CatalogConst.NameMinLength)]
        public string Name { get; set; } = string.Empty;


        public CreateCatalog CreateCommand(long userId)
        {
            return new CreateCatalog()
            {
                Name = Name,
                UserId = userId
            };
        }
    }
}