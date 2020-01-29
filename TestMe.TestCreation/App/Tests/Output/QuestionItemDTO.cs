using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Tests.Output
{
    public class QuestionItemDTO
    {
        public QuestionHeaderDTO? Question { get; set; }



        internal static readonly Expression<Func<QuestionItem, QuestionItemDTO>> MappingExpr = x =>
            new QuestionItemDTO
            {
                Question = new QuestionHeaderDTO()
                {
                    CatalogId = x.Question.CatalogId,
                    Content = x.Question.Content,
                    QuestionId = x.Question.QuestionId
                },
            };
    }
}
