using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs.Output
{
    public class CatalogDTO : CatalogHeaderDTO
    {



        internal new static readonly Expression<Func<Catalog, CatalogDTO>> Mapping = x =>
            new CatalogDTO
            {
                CatalogId = x.CatalogId,
                Name = x.Name
            };
    }
}
