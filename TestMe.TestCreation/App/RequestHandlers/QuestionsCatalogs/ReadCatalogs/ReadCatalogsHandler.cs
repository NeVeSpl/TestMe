using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs
{
    internal class ReadCatalogsHandler : IRequestHandler<ReadCatalogsQuery, Result<OffsetPagedResults<CatalogOnListDTO>>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadCatalogsHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }


        public async Task<Result<OffsetPagedResults<CatalogOnListDTO>>> Handle(ReadCatalogsQuery query, CancellationToken cancellationToken)
        {
            if (query.UserId != query.OwnerId)
            {
                return Result.Unauthorized();
            }

            var catalogs = await context.QuestionsCatalogs.Where(x => x.OwnerId == query.UserId)
                                                    .Skip(query.Pagination.Offset)
                                                    .Take(query.Pagination.Limit + 1)
                                                    .Select(CatalogOnListDTO.MappingExpr).ToListAsync();

            var pagedResult = new OffsetPagedResults<CatalogOnListDTO>(catalogs, query.Pagination.Limit);

            return Result.Ok(pagedResult);
        }
    }
}