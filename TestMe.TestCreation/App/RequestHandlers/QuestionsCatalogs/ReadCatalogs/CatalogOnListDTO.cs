using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs
{
    public class CatalogOnListDTO
    {
        public long CatalogId { get;  set; }
        public string Name { get; set; } = String.Empty;


        public CatalogOnListDTO()
        {

        }


        internal static readonly Expression<Func<Catalog, CatalogOnListDTO>> MappingExpr = x =>
            new CatalogOnListDTO
            {
                CatalogId = x.CatalogId,
                Name = x.Name
            };
    }
}
