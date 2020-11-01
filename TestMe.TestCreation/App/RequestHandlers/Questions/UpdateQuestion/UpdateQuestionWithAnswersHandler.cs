using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion
{
    internal sealed class UpdateQuestionWithAnswersHandler : IRequestHandler<UpdateQuestionWithAnswersCommand, Result>
    {
        private readonly ITestCreationUoW uow;


        public UpdateQuestionWithAnswersHandler(ITestCreationUoW uow)
        {
            this.uow = uow;
        }



        public async Task<Result> Handle(UpdateQuestionWithAnswersCommand command, CancellationToken cancellationToken)
        {
            Question question = uow.Questions.GetByIdWithAnswers(command.QuestionId);

            if (question == null)
            {
                return Result.NotFound();
            }
            if (question.OwnerId != command.UserId) // todo : check catalog instead
            {
                return Result.Unauthorized();
            }
            if (command.ConcurrencyToken.HasValue)
            {
                if (question.ConcurrencyToken != command.ConcurrencyToken.Value)
                {
                    return Result.Conflict();
                }
            }

            question.Content = command.Content;

            if (question.CatalogId != command.CatalogId)
            {
                var owner = uow.Owners.GetById(question.OwnerId);
                var policy = AddQuestionPolicyFactory.Create(owner.MembershipLevel);
                QuestionMover.MoveQuestionToCatalog(question, command.CatalogId, uow.QuestionsCatalogs, policy);
            }

            question.Answers.MergeWith(command.Answers, x => x.AnswerId, y => y.AnswerId,
                                       onAdd: x => question.AddAnswer(x.Content, x.IsCorrect),
                                       onUpdate: (x, y) => { x.Content = y.Content; x.IsCorrect = y.IsCorrect; },
                                       onDelete: y => question.DeleteAnswer(y));

            await uow.Save();

            return Result.Ok();
        }
    }
}
