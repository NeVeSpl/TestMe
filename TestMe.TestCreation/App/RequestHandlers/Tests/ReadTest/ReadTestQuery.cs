using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;


namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest
{
    public class ReadTestQuery : IRequest<Result<TestDTO>>, IHaveUserId
    {
        public long UserId
        {
            get;
            set;
        }

        public long TestId
        {
            get;
            set;
        }

        public ReadTestQuery(long testId)
        {           
            TestId = testId;
        }
    }
}