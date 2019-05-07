using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
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
        public string Content { get; set; }
      
        public long CatalogId { get;  set; }


        internal static readonly Expression<Func<Question, QuestionHeaderDTO>> Mapping = x =>
           new QuestionHeaderDTO
           {
               QuestionId = x.QuestionId,
               Content = x.Content,
               CatalogId = x.CatalogId,
           };
    }
}
