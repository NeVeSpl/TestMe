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
               

        public Result<List<CatalogHeaderDTO>> GetCatalogHeaders(long userId, long ownerId)
        {
            if (userId != ownerId)
            {
                return Result.Unauthorized();
            }
            var catalogs = context.QuestionsCatalogs.Where(x => x.OwnerId == userId).Select(CatalogHeaderDTO.MappingExpr).ToList();
            return Result.Ok(catalogs);
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

        public Result<QuestionsCatalogDTO> GetById(long userId, long catalogId)
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
