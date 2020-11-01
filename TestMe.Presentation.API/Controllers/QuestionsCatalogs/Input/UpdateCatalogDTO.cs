using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.UpdateCatalog;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input
{
    public class UpdateCatalogDTO 
    {
        [Required]
        [StringLength(maximumLength: CatalogConst.NameMaxLength, MinimumLength = CatalogConst.NameMinLength)]
        public string Name { get; set; } = string.Empty;
        

        public UpdateCatalogCommand CreateCommand(long catalogId)
        {
            return new UpdateCatalogCommand()
            {
                Name = Name,               
                CatalogId = catalogId
            };
        }
    }
}