using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTest
{
    public class DeleteTestCommand : IRequest<Result>, IHaveUserId
    {
        public long UserId { get; set; }
        public long TestId { get; set; }


        internal DeleteTestCommand()
        {

        }
        public DeleteTestCommand(long testId)
        {           
            TestId = testId;
        }
    }
}