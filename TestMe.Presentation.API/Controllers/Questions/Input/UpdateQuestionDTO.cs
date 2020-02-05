using TestMe.TestCreation.App.Questions.Input;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class UpdateQuestionDTO : QuestionBaseDTO<UpdateAnswerDTO>
    {
        /// <summary>
        /// When ConcurrencyToken is not provided update will be forced (it will succeed even if concurrent edit happened)
        /// </summary>
        public uint? ConcurrencyToken { get; set; }



        public UpdateQuestion CreateCommand(long questionId)
        {
            return new UpdateQuestion()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value,
              
                QuestionId = questionId,
                ConcurrencyToken = ConcurrencyToken
            };
        }
    }
}
