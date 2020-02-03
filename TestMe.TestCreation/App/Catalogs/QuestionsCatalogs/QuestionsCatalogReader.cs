using System.Collections.Generic;
using System.Linq;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Catalogs
{
    internal sealed class QuestionsCatalogReader
    {
        private readonly ReadOnlyTestCreationDbContext context;


        public QuestionsCatalogReader(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;           
        }
               

        public Result<OffsetPagedResults<CatalogHeaderDTO>> GetCatalogHeaders(long userId, long ownerId, OffsetPagination pagination)
        {
            if (userId != ownerId)
            {
                return Result.Unauthorized();
            }

            var catalogs = context.QuestionsCatalogs.Where(x => x.OwnerId == userId)
                                                    .Skip(pagination.Offset)
                                                    .Take(pagination.Limit + 1)
                                                    .Select(CatalogHeaderDTO.MappingExpr).ToList();

            var pagedResult = new OffsetPagedResults<CatalogHeaderDTO>(catalogs, pagination.Limit);

            return Result.Ok(pagedResult);
        }
        public Result<CatalogHeaderDTO> GetCatalogHeader(long userId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.Where(x => x.CatalogId == catalogId && x.OwnerId == userId).Select(CatalogHeaderDTO.MappingExpr).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }

        public Result<QuestionsCatalogDTO> GetCatalog(long userId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.Where(x => x.CatalogId == catalogId && x.OwnerId == userId).Select(QuestionsCatalogDTO.MappingExpr).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }
    }
}
