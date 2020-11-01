using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTestItem
{
    public class DeleteTestItemCommand : IRequest<Result>, IHaveUserId
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public long TestItemId { get; set; }


        public DeleteTestItemCommand()
        {
        }

        public DeleteTestItemCommand(long testId, long testItemId)
        {          
            TestId = testId;
            TestItemId = testItemId;
        }
    }
}