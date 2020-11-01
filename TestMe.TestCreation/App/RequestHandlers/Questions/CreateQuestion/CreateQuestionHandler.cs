using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.CreateQuestion
{
    internal sealed class CreateQuestionWithAnswersHandler : IRequestHandler<CreateQuestionWithAnswersCommand, Result<long>>
    {
        private readonly ITestCreationUoW uow;


        public CreateQuestionWithAnswersHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }


        public async Task<Result<long>> Handle(CreateQuestionWithAnswersCommand command, CancellationToken cancellationToken)
        {
            var catalog = uow.QuestionsCatalogs.GetById(command.CatalogId);

            if (catalog == null)
            {
                return Result.Error("Catalog not found");
            }
            if (catalog.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            var question = Question.Create(command.Content, command.UserId);

            if (command.Answers != null)
            {
                foreach (var answer in command.Answers)
                {
                    question.AddAnswer(answer.Content, answer.IsCorrect);
                }
            }

            var owner = uow.Owners.GetById(catalog.OwnerId);
            var policy = AddQuestionPolicyFactory.Create(owner.MembershipLevel);
            catalog.AddQuestion(question, policy);
            await uow.Save();

            return Result.Ok(question.QuestionId);
        }
    }
}
