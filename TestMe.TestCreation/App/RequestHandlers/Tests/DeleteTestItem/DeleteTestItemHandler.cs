using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.DeleteTestItem
{
    internal class DeleteTestItemHandler : IRequestHandler<DeleteTestItemCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public DeleteTestItemHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }

        public async Task<Result> Handle(DeleteTestItemCommand command, CancellationToken cancellationToken)
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

            test.RemoveQuestion(item);
            await uow.Save();

            return Result.Ok();
        }
    }
}