using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.DeleteCatalog
{
    public class DeleteCatalogCommand : IRequest<Result>, IHaveUserId
    {
        public long UserId { get; set; }
        public long CatalogId { get; set; }


        public DeleteCatalogCommand(long catalogId)
        {
           
            CatalogId = catalogId;
        }
    }
}