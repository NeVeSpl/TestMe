using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.CreateTest
{
    public class CreateTestCommand : IRequest<Result<long>>, IHaveUserId
    {
        public long UserId { get; set; }    
        public string Title { get; set; } = string.Empty;
        public long OwnerId { get; set; }
    }
}