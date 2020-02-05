using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.TestsCatalogs.Output
{
    public class TestsCatalogDTO : CatalogDTO
    {
       



        internal new static readonly Expression<Func<TestsCatalog, TestsCatalogDTO>> MappingExpr = x =>
           new TestsCatalogDTO
           {
               CatalogId = x.CatalogId,
               Name = x.Name,              
           };
    }
}
