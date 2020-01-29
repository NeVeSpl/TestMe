using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;

namespace TestMe.Presentation.API.Controllers.Questions.Input
{
    public abstract class QuestionBaseDTO<T> where T: CreateAnswerDTO
    {
        [StringLength(maximumLength: QuestionConst.ContentMaxLength)]
        public string Content { get; set; } = string.Empty;

        public List<T> Answers { get; set; } = new List<T>();

        [Required]
        public long? CatalogId { get; set; }
    }
}
