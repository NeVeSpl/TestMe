using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class UpdateQuestionDTO : QuestionBaseDTO<UpdateAnswerDTO>
    {
        /// <summary>
        /// When ConcurrencyToken is not provided update will be forced (it will succeed even if concurrent edit happened)
        /// </summary>
        public uint? ConcurrencyToken { get; set; }



        public UpdateQuestion CreateCommand(long userId, long questionId)
        {
            return new UpdateQuestion()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value,
                UserId = userId,
                QuestionId = questionId,
                ConcurrencyToken = ConcurrencyToken
            };
        }
    }
}
