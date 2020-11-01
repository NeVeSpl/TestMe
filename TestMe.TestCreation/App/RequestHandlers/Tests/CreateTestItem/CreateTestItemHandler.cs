using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Tests.CreateTestItem
{
    internal class CreateTestItemHandler : IRequestHandler<CreateTestItemCommand, Result<long>>
    {
        private readonly ITestCreationUoW uow;


        public CreateTestItemHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }


        public async Task<Result<long>> Handle(CreateTestItemCommand command, CancellationToken cancellationToken)
        {
            Test test = uow.Tests.GetByIdWithTestItems(command.TestId);
            Question question = uow.Questions.GetByIdWithAnswers(command.QuestionId);

            if (test == null)
            {
                return Result.NotFound();
            }
            if (question == null)
            {
                return Result.Error("Question not found");
            }
            if ((test.OwnerId != command.UserId) || (question.OwnerId != command.UserId))
            {
                return Result.Unauthorized();
            }

            QuestionItem item = test.AddQuestion(question);
            await uow.Save();

            return Result.Ok(item.QuestionItemId);
        }
    }
}