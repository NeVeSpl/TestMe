using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.CreateTest
{
    internal class CreateTestHandler : IRequestHandler<CreateTestCommand, Result<long>>
    {
        private readonly ITestCreationUoW uow;


        public CreateTestHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }

        public async Task<Result<long>> Handle(CreateTestCommand command, CancellationToken cancellationToken)
        {
            var owner = uow.Owners.GetByIdWithTestsCatalogs(command.OwnerId);
            var catalog = owner.TestsCatalogs.First();

            if (catalog == null)
            {
                return Result.Error("Catalog not found");
            }
            if (catalog.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            var test = Test.Create(command.UserId, command.Title);
            catalog.AddTest(test);
            await uow.Save();

            return Result.Ok(test.TestId);
        }
    }
}