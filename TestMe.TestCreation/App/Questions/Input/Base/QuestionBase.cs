using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;


namespace TestMe.TestCreation.App.Questions.Input
{
    public abstract class QuestionBase<T> where T: CreateAnswer
    {
        [StringLength(maximumLength: Question.ContentMaxLength)]
        public string Content { get; set; }

        public List<T> Answers { get; set; }

        [Required]
        public long? CatalogId { get; set; }
    }
}
