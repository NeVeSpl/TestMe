using System.Collections.Generic;
using System.Linq;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Persistence;


namespace TestMe.TestCreation.App.Catalogs
{
    internal sealed class TestsCatalogReader
    {
        private readonly ReadOnlyTestCreationDbContext context;


        public TestsCatalogReader(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;           
        }


        public Result<List<CatalogHeaderDTO>> GetTestsCatalogs(long userId, long ownerId)
        {
            if (userId != ownerId)
            {
                return Result.Unauthorized();
            }
            return Result.Ok(context.TestsCatalogs.Where(x => x.OwnerId == ownerId).Select(CatalogHeaderDTO.MappingExpr).ToList());
        }

        public CatalogDTO GetById(long catalogId)
        {
            return context.TestsCatalogs.Where(x => x.CatalogId == catalogId).Select(CatalogDTO.MappingExpr).FirstOrDefault();
        }
    }
}
