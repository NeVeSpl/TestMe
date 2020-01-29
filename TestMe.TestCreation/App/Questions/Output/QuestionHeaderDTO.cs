using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Output
{
    public class QuestionHeaderDTO
    {
        public long QuestionId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;




        internal static readonly Expression<Func<Question, QuestionHeaderDTO>> MappingExpr = x =>
           new QuestionHeaderDTO
           {
               QuestionId = x.QuestionId,
               Content = x.Content,           
           };
    }
}
