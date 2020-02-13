using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.Questions.Input;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public class CreateQuestionDTO
    {
        [StringLength(maximumLength: QuestionConst.ContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        public List<CreateAnswerDTO> Answers { get; set; } = new List<CreateAnswerDTO>();

        [Required]
        public long? CatalogId { get; set; }

        public CreateQuestion CreateCommand()
        {
            return new CreateQuestion()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value, 
            };
        }
    }
}
