using TestMe.TestCreation.App.TestsCatalogs.Input;

namespace TestMe.Presentation.API.Controllers.TestsCatalogs.Input
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
