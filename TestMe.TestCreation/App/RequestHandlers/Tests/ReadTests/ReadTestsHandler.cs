using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests
{
    internal class ReadTestsHandler : IRequestHandler<ReadTestsQuery, Result<OffsetPagedResults<TestOnListDTO>>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadTestsHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }


        public async Task<Result<OffsetPagedResults<TestOnListDTO>>> Handle(ReadTestsQuery query, CancellationToken cancellationToken)
        {          
            if (query.OwnerId != query.UserId)
            {
                return Result.Unauthorized();
            }

            var tests = await context.Tests.Where(x => x.OwnerId == query.OwnerId)
                                     .Select(x => new TestOnListDTO
                                                   {
                                                       TestId = x.TestId,
                                                       Title = x.Title
                                                   })
                                     .Skip(query.Pagination.Offset)
                                     .Take(query.Pagination.Limit + 1)
                                     .ToListAsync();

            return Result.Ok(new OffsetPagedResults<TestOnListDTO>(tests, query.Pagination.Limit));
        }
    }
}