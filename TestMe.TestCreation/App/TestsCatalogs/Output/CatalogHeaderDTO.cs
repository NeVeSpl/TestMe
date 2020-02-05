using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.TestsCatalogs.Output
{
    public class CatalogHeaderDTO
    {
        public long CatalogId { get;  set; }
        public string Name { get; set; } = String.Empty;


        public CatalogHeaderDTO()
        {

        }


        internal static readonly Expression<Func<Catalog, CatalogHeaderDTO>> MappingExpr = x =>
            new CatalogHeaderDTO
            {
                CatalogId = x.CatalogId,
                Name = x.Name
            };
    }
}
