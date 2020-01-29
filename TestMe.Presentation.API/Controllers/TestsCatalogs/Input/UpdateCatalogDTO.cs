using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMe.TestCreation.App.Catalogs.Input;

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
