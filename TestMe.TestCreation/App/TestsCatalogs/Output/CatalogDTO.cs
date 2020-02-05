﻿using System;
using System.Linq.Expressions;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.TestsCatalogs.Output
{
    public class CatalogDTO : CatalogHeaderDTO
    {



        internal new static readonly Expression<Func<Catalog, CatalogDTO>> MappingExpr = x =>
            new CatalogDTO
            {
                CatalogId = x.CatalogId,
                Name = x.Name
            };
    }
}