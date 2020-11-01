using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.UpdateTestItem
{
    internal class UpdateTestItemHandler : IRequestHandler<UpdateTestItemCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public UpdateTestItemHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }


        public async Task<Result> Handle(UpdateTestItemCommand command, CancellationToken cancellationToken)
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

            QuestionItem item = test.Questions.FirstOrDefault(x => x.QuestionItemId == command.TestItemId);

            if (item == null)
            {
                return Result.NotFound();
            }
            // todo : update test item

            await uow.Save();

            return Result.Ok();
        }
    }
}