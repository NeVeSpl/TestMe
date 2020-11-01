using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTest
{
    internal class UpdateTestHandler : IRequestHandler<UpdateTestCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public UpdateTestHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }

        public async Task<Result> Handle(UpdateTestCommand command, CancellationToken cancellationToken)
        {
            Test test = uow.Tests.GetByIdWithTestItems(command.TestId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (test.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            test.Title = command.Title;     
            await uow.Save();

            return Result.Ok();
        }
    }
}