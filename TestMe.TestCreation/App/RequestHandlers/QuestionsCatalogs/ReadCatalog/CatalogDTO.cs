using System;
using System.Linq.Expressions;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog
{
    public class CatalogDTO 
    {
        public long CatalogId { get; set; }
        public string Name { get; set; } = String.Empty;
        public int QuestionsCount { get; set; }



        internal static readonly Expression<Func<Domain.QuestionsCatalog, CatalogDTO>> MappingExpr = x =>
           new CatalogDTO
           {
               CatalogId = x.CatalogId,
               Name = x.Name,
               QuestionsCount = x.QuestionsCount
           };
    }
}
