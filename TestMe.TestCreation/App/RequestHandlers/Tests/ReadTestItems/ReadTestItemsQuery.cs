using System.Collections.Generic;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTestItems
{
    public class ReadTestItemsQuery : IRequest<Result<List<TestItemDTO>>>, IHaveUserId
    {
        public long UserId { get; set; }
        public long TestId { get; set; }

        public ReadTestItemsQuery(long testId)
        {
            TestId = testId;
        }
    }
}