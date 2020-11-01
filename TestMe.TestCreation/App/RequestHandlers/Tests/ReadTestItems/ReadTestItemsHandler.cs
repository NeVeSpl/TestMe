using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems
{
    internal class ReadTestItemsHandler : IRequestHandler<ReadTestItemsQuery, Result<List<TestItemDTO>>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadTestItemsHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }




        public async Task<Result<List<TestItemDTO>>> Handle(ReadTestItemsQuery query, CancellationToken cancellationToken)
        {
            var test = await context.Tests.Where(x => x.TestId == query.TestId)
                                          .Include(x => x.Questions)
                                          .ThenInclude(x => x.Question)                                         
                                          .IgnoreQueryFilters()
                                          .FirstOrDefaultAsync();

            if (test == null)
            {
                return Result.NotFound();
            }

            if (test.OwnerId != query.UserId)
            {
                return Result.Unauthorized();
            }

            var positions = test.Questions.Select(x => new TestItemDTO() { QuestionId = x.Question.QuestionId, TestItemId = x.QuestionItemId }).ToList();

            return Result.Ok(positions);
        }
    }
}