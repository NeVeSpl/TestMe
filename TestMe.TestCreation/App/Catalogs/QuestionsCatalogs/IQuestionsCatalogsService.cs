using System.Collections.Generic;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;

namespace TestMe.TestCreation.App.Catalogs
{
    public interface IQuestionsCatalogsService
    {
        Result<long> CreateCatalog(CreateCatalog createCatalog);
        Result DeleteCatalog(DeleteCatalog deleteCatalog);
        Result UpdateCatalog(UpdateCatalog updateCatalog);

        Result<List<CatalogHeaderDTO>> ReadCatalogHeaders(long ownerId);
        Result<CatalogHeaderDTO> ReadCatalogHeader(long ownerId, long catalogId);
        Result<QuestionsCatalogDTO> ReadCatalog(long owner, long catalogId);
    }
}