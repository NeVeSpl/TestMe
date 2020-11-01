using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.DeleteQuestion
{
    public sealed class DeleteQuestionWithAnswersCommand : IHaveUserId, IRequest<Result>
    {
        public long UserId { get; set; }
        public long QuestionId { get; set; }


        internal DeleteQuestionWithAnswersCommand()
        {
        }

        public DeleteQuestionWithAnswersCommand(long questionId)
        {
            QuestionId = questionId;
        }
    }
}