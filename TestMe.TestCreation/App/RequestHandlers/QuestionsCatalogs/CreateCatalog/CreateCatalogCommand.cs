using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.CreateCatalog
{
    public class CreateCatalogCommand : IRequest<Result<long>>, IHaveUserId
    {
        public long UserId { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}