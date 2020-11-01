using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.UpdateCatalog
{
    public class UpdateCatalogCommand : IRequest<Result>, IHaveUserId
    {
        public long CatalogId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}