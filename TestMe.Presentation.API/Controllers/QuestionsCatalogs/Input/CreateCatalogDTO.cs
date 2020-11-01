using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.CreateCatalog;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input
{
    public class CreateCatalogDTO
    {
        [Required]
        [StringLength(maximumLength: CatalogConst.NameMaxLength, MinimumLength = CatalogConst.NameMinLength)]
        public string Name { get; set; } = string.Empty;

        public long OwnerId { get; set; }


        public CreateCatalogCommand CreateCommand()
        {
            return new CreateCatalogCommand()
            {
                Name = Name,
                OwnerId = OwnerId
            };
        }
    }
}