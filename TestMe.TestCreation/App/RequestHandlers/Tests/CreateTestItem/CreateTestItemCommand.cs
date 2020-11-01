using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.CreateTestItem
{
    public class CreateTestItemCommand : IRequest<Result<long>>, IHaveUserId
    {
        public long UserId { get; set; }
        public long TestId { get; set; }
        public long QuestionId { get; set; }


        internal CreateTestItemCommand()
        {

        }

        public CreateTestItemCommand(long testId, long questionId)
        {
            TestId = testId;
            QuestionId = questionId;
        }
    }
}