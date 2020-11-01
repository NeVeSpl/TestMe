using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.DeleteQuestion
{
    internal sealed class DeleteQuestionWithAnswersHandler : IRequestHandler<DeleteQuestionWithAnswersCommand, Result>
    {
        private readonly ITestCreationUoW uow;

        public DeleteQuestionWithAnswersHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }


        public async Task<Result> Handle(DeleteQuestionWithAnswersCommand command, CancellationToken cancellationToken)
        {
            var question = uow.Questions.GetByIdWithAnswers(command.QuestionId);

            if (question == null)
            {
                return Result.NotFound();
            }

            var catalog = uow.QuestionsCatalogs.GetById(question.CatalogId);

            if (catalog.OwnerId != command.UserId)
            {
                return Result.Unauthorized();
            }

            catalog.DeleteQuestion(question);
            await uow.Save();

            return Result.Ok();
        }
    }
}