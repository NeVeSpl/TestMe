using System.Collections.Generic;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;

namespace TestMe.TestCreation.App.Catalogs
{
    public interface ITestsCatalogsService
    {
        Result<long> CreateCatalog(long ownerId, CreateCatalog createCatalog);
        Result DeleteCatalog(long ownerId, long catalogId);
        Result<List<CatalogHeaderDTO>> ReadCatalogHeaders(long ownerId);
        Result<CatalogDTO> ReadCatalog(long owner, long catalogId);
        Result UpdateCatalog(long ownerId, long catalogId, UpdateCatalog updateCatalog);
    }
}