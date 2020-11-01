using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTests
{
    public class ReadTestsQuery : IRequest<Result<OffsetPagedResults<TestOnListDTO>>>, IHaveUserId
    {
        public long UserId
        {
            get; set;
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

        public ReadTestsQuery(long ownerId, OffsetPagination pagination)
        {
            OwnerId = ownerId;
          
            Pagination = pagination;
        }
    }
}