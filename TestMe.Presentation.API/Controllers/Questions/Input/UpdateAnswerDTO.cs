using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.Questions.UpdateQuestion;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class UpdateAnswerDTO : CreateAnswerDTO
    {
        [Required]
        public long? AnswerId
        {
            get;
            set;
        }


        public new UpdateAnswer CreateAnswer()
        {
            return new UpdateAnswer()
            {
                Content = Content,
                IsCorrect = IsCorrect,
                AnswerId = AnswerId!.Value
            };
        }
    }
}
