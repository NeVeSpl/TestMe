using System.Collections.Generic;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.TestsCatalogs.Input;
using TestMe.TestCreation.App.TestsCatalogs.Output;

namespace TestMe.TestCreation.App.TestsCatalogs
{
    public interface ITestsCatalogsService
    {
        Result<long> CreateCatalog(CreateCatalog createCatalog);
        Result DeleteCatalog(DeleteCatalog deleteCatalog);
        Result UpdateCatalog(UpdateCatalog updateCatalog);

        Result<OffsetPagedResults<CatalogHeaderDTO>> ReadCatalogHeaders(long userId, long ownerId, OffsetPagination pagination);
        Result<CatalogDTO> ReadCatalog(long userId, long catalogId);        
    }
}