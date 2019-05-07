using System.Collections.Generic;
using TestMe.SharedKernel.App;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs
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


        public Result<List<CatalogHeaderDTO>> ReadCatalogHeaders(long ownerId)
        {
            return Result.Ok(catalogReader.GetTestsCatalogs(ownerId));
        }

        public Result<CatalogDTO> ReadCatalog(long owner, long catalogId)
        {
            // todo : check if owner has access to given catalog
            var catalog = catalogReader.GetById(catalogId);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            return Result.Ok(catalog);
        }

        public Result<long> CreateCatalog(long ownerId, CreateCatalog createCatalog)
        {
            Owner owner = uow.Owners.GetById(ownerId);

            TestsCatalog catalog = owner.AddTestsCatalog(createCatalog.Name);    
            uow.Save();

            return Result.Ok(catalog.CatalogId);
        }

        public Result UpdateCatalog(long ownerId, long catalogId, UpdateCatalog updateCatalog)
        {
            var catalog = uow.TestsCatalogs.GetById(catalogId);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            catalog.Name = updateCatalog.Name;
            uow.Save();

            return Result.Ok();
        }

        public Result DeleteCatalog(long ownerId, long catalogId)
        {
            var catalog = uow.TestsCatalogs.GetById(catalogId, includeTests: true);

            if (catalog == null)
            {
                return Result.NotFound();
            }

            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            catalog.Delete();           
            uow.Save();

            return Result.Ok();
        }
    }
}