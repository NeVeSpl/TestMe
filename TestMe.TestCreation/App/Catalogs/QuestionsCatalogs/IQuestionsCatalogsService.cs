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

        Result<OffsetPagedResults<CatalogHeaderDTO>> ReadCatalogHeaders(long userId, long ownerId, OffsetPagination pagination);
        Result<CatalogHeaderDTO> ReadCatalogHeader(long userId, long catalogId);
        Result<QuestionsCatalogDTO> ReadCatalog(long userId, long catalogId);
    }
}