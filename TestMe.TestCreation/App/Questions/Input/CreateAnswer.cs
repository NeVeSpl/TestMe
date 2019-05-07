using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Input
{
    public class CreateAnswer
    {  
        [Required]
        [StringLength(maximumLength: Answer.ContentMaxLength)]
        public string Content { get; set; }

        public bool IsCorrect { get; set; }
    }
}
