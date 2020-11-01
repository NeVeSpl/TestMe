using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTest
{
    internal class DeleteTestHandler : IRequestHandler<DeleteTestCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public DeleteTestHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }

        public async Task<Result> Handle(DeleteTestCommand command, CancellationToken cancellationToken)
        {
            var test = uow.Tests.GetByIdWithTestItems(command.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            test.Delete();
            await uow.Save();

            return Result.Ok();
        }
    }
}