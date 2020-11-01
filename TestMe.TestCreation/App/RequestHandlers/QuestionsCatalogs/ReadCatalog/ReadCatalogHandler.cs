using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog
{
    internal class ReadCatalogHandler : IRequestHandler<ReadCatalogQuery, Result<CatalogDTO>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadCatalogHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }



        public async Task<Result<CatalogDTO>> Handle(ReadCatalogQuery query, CancellationToken cancellationToken)
        {            
            var catalog = await context.QuestionsCatalogs
                                       .Where(x => x.CatalogId == query.CatalogId && x.OwnerId == query.UserId)
                                       .Select(CatalogDTO.MappingExpr)
                                       .FirstOrDefaultAsync();

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }
    }
}