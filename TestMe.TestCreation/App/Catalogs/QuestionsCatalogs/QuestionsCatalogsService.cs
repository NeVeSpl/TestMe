using System.Collections.Generic;
using TestMe.BuildingBlocks.App;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;
using TestMe.TestCreation.Domain;

namespace TestMe.TestCreation.App.Catalogs
{
    internal sealed class QuestionsCatalogsService : IQuestionsCatalogsService
    {
        private readonly QuestionsCatalogReader catalogReader;
        private readonly ITestCreationUoW uow;


        public QuestionsCatalogsService(QuestionsCatalogReader catalogReader, ITestCreationUoW uow)
        {
            this.catalogReader = catalogReader;
            this.uow = uow;
        }


        public Result<OffsetPagedResults<CatalogHeaderDTO>> ReadCatalogHeaders(long userId, long ownerId, OffsetPagination pagination)
        {            
            return catalogReader.GetCatalogHeaders(userId, ownerId, pagination);
        }

        public Result<CatalogHeaderDTO> ReadCatalogHeader(long userId, long catalogId)
        {          
            return catalogReader.GetCatalogHeader(userId, catalogId);
        }

        public Result<QuestionsCatalogDTO> ReadCatalog(long userId, long catalogId)
        {            
            return catalogReader.GetCatalog(userId, catalogId);
        }

        public Result<long> CreateCatalog(CreateCatalog createCatalog)
        {
            Owner owner = uow.Owners.GetById(createCatalog.UserId);

            var policy = AddQuestionsCatalogPolicyFactory.Create(owner.MembershipLevel);
            QuestionsCatalog catalog = owner.AddQuestionsCatalog(createCatalog.Name, policy);
            uow.Save();

            return Result.Ok(catalog.CatalogId);
        }

        public Result UpdateCatalog(UpdateCatalog updateCatalog)
        {          
            QuestionsCatalog catalog = uow.QuestionsCatalogs.GetById(updateCatalog.CatalogId);

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
            var owner = uow.Owners.GetById(deleteCatalog.UserId);
            var catalog = uow.QuestionsCatalogs.GetById(deleteCatalog.CatalogId, includeQuestions: true);        

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != deleteCatalog.UserId)
            {
                return Result.Unauthorized();
            }

            owner.DeleteQuestionsCatalog(catalog); 
            uow.Save();

            return Result.Ok();
        }
    }
}