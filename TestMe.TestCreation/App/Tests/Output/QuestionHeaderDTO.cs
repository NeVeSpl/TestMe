using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Output
{
    public class QuestionHeaderDTO
    {
        public long QuestionId
        {
            get;
            set;
        }
        public string Content { get; set; } = string.Empty;

        public long CatalogId { get;  set; }


        internal static readonly Expression<Func<Question, QuestionHeaderDTO>> MappingExpr = x =>
           new QuestionHeaderDTO
           {
               QuestionId = x.QuestionId,
               Content = x.Content,
               CatalogId = x.CatalogId,
           };
    }
}
