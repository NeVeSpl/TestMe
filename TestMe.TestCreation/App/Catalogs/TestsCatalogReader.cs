using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Persistence;


namespace TestMe.TestCreation.App.Catalogs
{
    internal sealed class TestsCatalogReader
    {
        private readonly ReadOnlyTestCreationDbContext context;


        public TestsCatalogReader(ReadOnlyTestCreationDbContext context)
        {
            this.context = context;           
        }


        public List<CatalogHeaderDTO> GetTestsCatalogs(long ownerId)
        {
            return context.TestsCatalogs.Where(x => x.OwnerId == ownerId).Select(CatalogHeaderDTO.MappingExpr).ToList();
        }

        public CatalogDTO GetById(long catalogId)
        {
            return context.TestsCatalogs.Where(x => x.CatalogId == catalogId).Select(CatalogDTO.MappingExpr).FirstOrDefault();
        }
    }
}
