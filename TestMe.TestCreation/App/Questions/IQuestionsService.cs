using System.Collections.Generic;
using System.Threading.Tasks;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.App.Questions.Output;

namespace TestMe.TestCreation.App.Questions
{
    public interface IQuestionsService
    {
        Result<long> CreateQuestionWithAnswers(CreateQuestion createQuestion);
        Result UpdateQuestionWithAnswers(UpdateQuestion updateQuestion);
        Result DeleteQuestionWithAnswers(DeleteQuestion deleteQuestion);

        Result<List<QuestionHeaderDTO>> ReadQuestionHeaders(long ownerId, long catalogId);
        Task<Result<List<QuestionHeaderDTO>>> ReadQuestionHeadersAsync(long ownerId, long catalogId);
        Result<QuestionHeaderDTO> ReadQuestionHeader(long ownerId, long questionId);
        Result<QuestionDTO> ReadQuestionWithAnswers(long ownerId, long questionId);
        Task<Result<QuestionDTO>> ReadQuestionWithAnswersAsync(long ownerId, long questionId);               
    }
}