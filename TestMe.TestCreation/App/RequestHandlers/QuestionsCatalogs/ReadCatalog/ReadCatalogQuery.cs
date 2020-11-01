using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog
{
    public class ReadCatalogQuery : IRequest<Result<CatalogDTO>>, IHaveUserId
    {
        public long UserId
        {
            get;
            set;
        }

        public long CatalogId
        {
            get;
            set;
        }

        public ReadCatalogQuery(long catalogId)
        {           
            CatalogId = catalogId;
        }
    }
}