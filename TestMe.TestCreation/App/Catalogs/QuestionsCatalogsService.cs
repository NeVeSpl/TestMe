using System.Collections.Generic;
using TestMe.SharedKernel.App;
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


        public Result<List<CatalogHeaderDTO>> ReadCatalogHeaders(long ownerId)
        {            
            return catalogReader.GetCatalogHeaders(ownerId);
        }

        public Result<CatalogHeaderDTO> ReadCatalogHeader(long ownerId, long catalogId)
        {          
            return catalogReader.GetCatalogHeader(ownerId, catalogId);
        }

        public Result<QuestionsCatalogDTO> ReadCatalog(long ownerId, long catalogId)
        {            
            return catalogReader.GetById(ownerId, catalogId);
        }

        public Result<long> CreateCatalog(long ownerId, CreateCatalog createCatalog)
        {
            Owner owner = uow.Owners.GetById(ownerId);

            var policy = AddQuestionsCatalogPolicyFactory.Create(MembershipLevel.Regular);
            QuestionsCatalog catalog = owner.AddQuestionsCatalog(createCatalog.Name, policy);
            uow.Save();

            return Result.Ok(catalog.CatalogId);
        }

        public Result UpdateCatalog(long ownerId, long catalogId, UpdateCatalog updateCatalog)
        {          
            QuestionsCatalog catalog = uow.QuestionsCatalogs.GetById(catalogId);

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
            var owner = uow.Owners.GetById(ownerId);
            var catalog = uow.QuestionsCatalogs.GetById(catalogId, includeQuestions: true);        

            if (catalog == null)
            {
                return Result.NotFound();
            }
            if (catalog.OwnerId != ownerId)
            {
                return Result.Unauthorized();
            }

            owner.DeleteQuestionsCatalog(catalog); 
            uow.Save();

            return Result.Ok();
        }
    }
}