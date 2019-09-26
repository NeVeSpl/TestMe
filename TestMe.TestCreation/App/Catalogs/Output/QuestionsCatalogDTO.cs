using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs.Output
{
    public class QuestionsCatalogDTO : CatalogDTO
    {
        public int QuestionsCount { get; set; }



        internal new static readonly Expression<Func<QuestionsCatalog, QuestionsCatalogDTO>> MappingExpr = x =>
           new QuestionsCatalogDTO
           {
               CatalogId = x.CatalogId,
               Name = x.Name,
               QuestionsCount = x.QuestionsCount
           };
    }
}
