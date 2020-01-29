using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestMe.TestCreation.Domain;


namespace TestMe.TestCreation.App.Questions.Input
{
    public abstract class QuestionBase<T> where T: CreateAnswer
    {
        public long UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public List<T> Answers { get; set; } = new List<T>();     
        public long CatalogId { get; set; }
    }
}
