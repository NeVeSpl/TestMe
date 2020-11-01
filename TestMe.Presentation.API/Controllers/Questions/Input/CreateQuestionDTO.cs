using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.App.RequestHandlers.Questions.CreateQuestion;
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

        public CreateQuestionWithAnswersCommand CreateCommand()
        {
            return new CreateQuestionWithAnswersCommand()
            {
                Content = Content,
                Answers = Answers.ConvertAll(thisAnswer => thisAnswer.CreateAnswer()),
                CatalogId = CatalogId!.Value, 
            };
        }
    }
}
