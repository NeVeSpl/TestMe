using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input;
using TestMe.TestCreation.App.QuestionsCatalogs;
using TestMe.TestCreation.App.QuestionsCatalogs.Input;
using TestMe.TestCreation.App.QuestionsCatalogs.Output;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs
{
    [Route("[controller]")]
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class QuestionsCatalogsController : Controller
    {
        private readonly IQuestionsCatalogsService service;


        public QuestionsCatalogsController(IQuestionsCatalogsService service)
        {
            this.service = service;
        }

        
        [HttpGet("headers")]       
        public ActionResult<OffsetPagedResults<CatalogHeaderDTO>> ReadQuestionsCatalogHeaders(long ownerId, [FromQuery]OffsetPagination pagination)
        {
            var result = service.ReadCatalogHeaders(UserId, ownerId, pagination);
            return ActionResult(result);
        }

        [HttpGet("{catalogId}/header")]
        public ActionResult<CatalogHeaderDTO> ReadQuestionsCatalogHeader(long catalogId)
        {
            var result = service.ReadCatalogHeader(UserId, catalogId);
            return ActionResult(result);
        }

        [HttpGet("{catalogId}")] 
        public ActionResult<QuestionsCatalogDTO> ReadQuestionsCatalog(long catalogId)
        {
            var result = service.ReadCatalog(UserId, catalogId);
            return ActionResult(result);
        }

        [HttpPost]      
        public ActionResult<long> CreateCatalog(CreateCatalogDTO createCatalog)
        {            
            var result = service.CreateCatalog(createCatalog.CreateCommand(UserId));
            return ActionResult(result);
        } 

        [HttpPut("{catalogId}")]       
        public ActionResult UpdateCatalog(long catalogId, UpdateCatalogDTO updateCatalog)
        {            
            var result = service.UpdateCatalog(updateCatalog.CreateCommand(UserId, catalogId));
            return ActionResult(result);
        }
        
        [HttpDelete("{catalogId}")]      
        public ActionResult DeleteCatalog(long catalogId)
        {
            var result = service.DeleteCatalog(new DeleteCatalog(UserId, catalogId));
            return ActionResult(result);
        }
    }
}