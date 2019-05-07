using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Persistence;

namespace TestMe.TestCreation.App.Catalogs
{
    internal sealed class QuestionsCatalogReader
    {
        private readonly TestCreationDbContext context;


        public QuestionsCatalogReader(TestCreationDbContext context)
        {
            this.context = context;           
        }
               

        public Result<List<CatalogHeaderDTO>> GetCatalogHeaders(long ownerId)
        {
            var catalogs = context.QuestionsCatalogs.AsNoTracking().Where(x => x.OwnerId == ownerId).Select(CatalogHeaderDTO.Mapping).ToList();
            return Result.Ok(catalogs);
        }
        public Result<CatalogHeaderDTO> GetCatalogHeader(long ownerId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.AsNoTracking().Where(x => x.CatalogId == catalogId && x.OwnerId == ownerId).Select(CatalogHeaderDTO.Mapping).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }

        public Result<QuestionsCatalogDTO> GetById(long ownerId, long catalogId)
        {
            var catalog = context.QuestionsCatalogs.AsNoTracking().Where(x => x.CatalogId == catalogId && x.OwnerId == ownerId).Select(QuestionsCatalogDTO.Mapping).FirstOrDefault();

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }
    }
}
