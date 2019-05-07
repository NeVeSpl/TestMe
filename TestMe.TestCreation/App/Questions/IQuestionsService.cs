using System.Collections.Generic;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.App.Questions.Output;

namespace TestMe.TestCreation.App.Questions
{
    public interface IQuestionsService
    {
        Result<long> CreateQuestionWithAnswers(long ownerId, CreateQuestion createQuestion);
        Result DeleteQuestionWithAnswers(long ownerId, long questionId);
        Result<List<QuestionHeaderDTO>> ReadQuestionHeaders(long ownerId, long catalogId);
        Result<QuestionHeaderDTO> ReadQuestionHeader(long ownerId, long questionId);
        Result<QuestionDTO> ReadQuestionWithAnswers(long ownerId, long questionId);
        Result UpdateQuestionWithAnswers(long ownerId, long questionId, UpdateQuestion updateQuestion);       
    }
}