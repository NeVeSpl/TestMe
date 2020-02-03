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


        public Result<OffsetPagedResults<CatalogHeaderDTO>> GetTestsCatalogs(long userId, long ownerId, OffsetPagination pagination)
        {
            if (userId != ownerId)
            {
                return Result.Unauthorized();
            }

            var catalogs = context.TestsCatalogs.Where(x => x.OwnerId == userId)
                                                .Skip(pagination.Offset)
                                                .Take(pagination.Limit + 1)
                                                .Select(CatalogHeaderDTO.MappingExpr).ToList();

            var pagedResult = new OffsetPagedResults<CatalogHeaderDTO>(catalogs, pagination.Limit);

            return Result.Ok(pagedResult);
        }

        public CatalogDTO GetById(long catalogId)
        {
            return context.TestsCatalogs.Where(x => x.CatalogId == catalogId).Select(CatalogDTO.MappingExpr).FirstOrDefault();
        }
    }
}
