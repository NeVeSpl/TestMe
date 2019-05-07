using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs.Output
{
    public class CatalogHeaderDTO
    {
        public long CatalogId { get;  set; }
        public string Name { get;  set; }


        public CatalogHeaderDTO()
        {

        }


        internal static readonly Expression<Func<Catalog, CatalogHeaderDTO>> Mapping = x =>
            new CatalogHeaderDTO
            {
                CatalogId = x.CatalogId,
                Name = x.Name
            };
    }
}
