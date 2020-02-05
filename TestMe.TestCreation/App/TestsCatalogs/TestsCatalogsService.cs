﻿using System.Collections.Generic;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.TestsCatalogs.Input;
using TestMe.TestCreation.App.TestsCatalogs.Output;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.TestsCatalogs
{
    internal sealed class TestsCatalogsService : ITestsCatalogsService
    {
        private readonly TestsCatalogReader catalogReader;
        private readonly ITestCreationUoW uow;


        public TestsCatalogsService(TestsCatalogReader catalogReader, ITestCreationUoW uow)
        {
            this.catalogReader = catalogReader;
            this.uow = uow;
        }


        public Result<OffsetPagedResults<CatalogHeaderDTO>> ReadCatalogHeaders(long userId, long ownerId, OffsetPagination pagination)
        {
            return catalogReader.GetTestsCatalogs(userId, ownerId, pagination);
        }

        public Result<CatalogDTO> ReadCatalog(long userId, long catalogId)
        {
            // todo : check if owner has access to given catalog
            var catalog = catalogReader.GetById(catalogId);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }

        public Result<long> CreateCatalog(CreateCatalog createCatalog)
        {
            Owner owner = uow.Owners.GetById(createCatalog.UserId);

            TestsCatalog catalog = owner.AddTestsCatalog(createCatalog.Name);    
            uow.Save();

            return Result.Ok(catalog.CatalogId);
        }

        public Result UpdateCatalog(UpdateCatalog updateCatalog)
        {
            var catalog = uow.TestsCatalogs.GetById(updateCatalog.CatalogId);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            if (catalog.OwnerId != updateCatalog.UserId)
            {
                return Result.Unauthorized();
            }

            catalog.Name = updateCatalog.Name;
            uow.Save();

            return Result.Ok();
        }

        public Result DeleteCatalog(DeleteCatalog deleteCatalog)
        {
            var catalog = uow.TestsCatalogs.GetById(deleteCatalog.CatalogId, includeTests: true);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            if (catalog.OwnerId != deleteCatalog.UserId)
            {
                return Result.Unauthorized();
            }

            catalog.Delete();           
            uow.Save();

            return Result.Ok();
        }
    }
}