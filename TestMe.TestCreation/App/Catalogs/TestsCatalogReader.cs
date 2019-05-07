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
        private readonly TestCreationDbContext context;


        public TestsCatalogReader(TestCreationDbContext context)
        {
            this.context = context;           
        }


        public List<CatalogHeaderDTO> GetTestsCatalogs(long ownerId)
        {
            return context.TestsCatalogs.AsNoTracking().Where(x => x.OwnerId == ownerId).Select(CatalogHeaderDTO.Mapping).ToList();
        }

        public CatalogDTO GetById(long catalogId)
        {
            return context.TestsCatalogs.AsNoTracking().Where(x => x.CatalogId == catalogId).Select(CatalogDTO.Mapping).FirstOrDefault();
        }
    }
}
