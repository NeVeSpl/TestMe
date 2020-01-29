using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class CreateAnswerDTO
    {  
        [Required]
        [StringLength(maximumLength: AnswerConst.ContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }



        public CreateAnswer CreateAnswer()
        {
            return new CreateAnswer()
            {
                Content = Content,
                IsCorrect = IsCorrect
            };
        }
    }
}
