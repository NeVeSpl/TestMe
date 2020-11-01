using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTestItem
{
    public class UpdateTestItemCommand : IRequest<Result>, IHaveUserId
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public long TestItemId { get; set; }
    }
}