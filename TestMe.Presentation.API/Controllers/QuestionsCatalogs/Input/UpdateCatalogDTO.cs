using TestMe.TestCreation.App.Catalogs.Input;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input
{
    public class UpdateCatalogDTO : CreateCatalogDTO
    {
        public UpdateCatalog CreateCommand(long userId, long catalogId)
        {
            return new UpdateCatalog()
            {
                Name = Name,
                UserId = userId,
                CatalogId = catalogId
            };
        }
    }
}