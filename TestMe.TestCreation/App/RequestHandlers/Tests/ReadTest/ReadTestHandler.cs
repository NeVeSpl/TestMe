using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.ReadTest
{
    internal class ReadTestHandler : IRequestHandler<ReadTestQuery, Result<TestDTO>>
    {
        private readonly ReadOnlyTestCreationDbContext context;

        public ReadTestHandler(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;
        }



        public async Task<Result<TestDTO>> Handle(ReadTestQuery query, CancellationToken cancellationToken)
        {
            Domain.Test test =  await context.Tests.Where(x => x.TestId == query.TestId).FirstOrDefaultAsync();

            if (test == null)
            {
                return Result.NotFound();
            }            

            if (test.OwnerId != query.UserId)
            {
                return Result.Unauthorized();
            }

            TestDTO dto = TestDTO.MapFrom(test);   
            return Result.Ok(dto);
        }
    }
}