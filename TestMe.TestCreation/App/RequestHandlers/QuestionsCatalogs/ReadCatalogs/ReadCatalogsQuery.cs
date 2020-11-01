using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs
{
    public class ReadCatalogsQuery : IRequest<Result<OffsetPagedResults<CatalogOnListDTO>>>, IHaveUserId
    {
        public long UserId
        {
            get;
            set;
        }

        public long OwnerId
        {
            get;
            set;
        }

        public OffsetPagination Pagination
        {
            get;
            set;
        }

        public ReadCatalogsQuery(long ownerId, OffsetPagination pagination)
        {          
            OwnerId = ownerId;
            Pagination = pagination;
        }
    }
}