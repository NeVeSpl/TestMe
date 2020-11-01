using MediatR;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.TestCreation.App.RequestHandlers.Questions.ReadQuestion
{
    public sealed class ReadQuestionWithAnswersQuery : IHaveUserId, IRequest<Result<QuestionWithAnswersDTO>>
    {
        public long UserId { get; set; }
        public long QuestionId { get; set; }

        public ReadQuestionWithAnswersQuery(long questionId)
        {           
            QuestionId = questionId;
        }       
    }
}