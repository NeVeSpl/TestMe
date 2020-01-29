using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Questions.Output
{
    public class AnswerDTO
    {
        public long AnswerId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }


        internal static readonly Expression<Func<Answer, AnswerDTO>> MappingExpr = x =>
         new AnswerDTO
         {
             AnswerId = x.AnswerId,
             Content = x.Content,
             IsCorrect = x.IsCorrect,
         };
        internal static readonly Func<Answer, AnswerDTO> Mapping = MappingExpr.Compile();
    }
}
