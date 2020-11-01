using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTest
{
    public class UpdateTestCommand : IRequest<Result>, IHaveUserId
    {
        public long TestId { get; set; }
        public long UserId { get; set; }      
        public string Title { get; set; } = string.Empty;
    }
}