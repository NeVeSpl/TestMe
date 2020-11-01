using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Controllers.QuestionsCatalogs.Input;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.DeleteCatalog;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalog;
using TestMe.TestCreation.App.RequestHandlers.QuestionsCatalogs.ReadCatalogs;

namespace TestMe.Presentation.API.Controllers.QuestionsCatalogs
{
    [Route("[controller]")]
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class QuestionsCatalogsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<OffsetPagedResults<CatalogOnListDTO>>> ReadQuestionsCatalogs(long ownerId, [FromQuery]OffsetPagination pagination)
        {
            var result = await Send(new ReadCatalogsQuery(ownerId, pagination));
            return ActionResult(result);
        }      

        [HttpGet("{catalogId}")] 
        public async Task<ActionResult<CatalogDTO>> ReadQuestionsCatalog(long catalogId)
        {
            var result = await Send(new ReadCatalogQuery(catalogId));
            return ActionResult(result);
        }

        [HttpPost]      
        public async Task<ActionResult<long>> CreateCatalog(CreateCatalogDTO createCatalog)
        {            
            var result = await Send(createCatalog.CreateCommand());
            return ActionResult(result);
        } 

        [HttpPut("{catalogId}")]       
        public async Task<ActionResult> UpdateCatalog(long catalogId, UpdateCatalogDTO updateCatalog)
        {            
            var result = await Send(updateCatalog.CreateCommand(catalogId));
            return ActionResult(result);
        }
        
        [HttpDelete("{catalogId}")]      
        public async Task<ActionResult> DeleteCatalog(long catalogId)
        {         
            var result = await Send(new DeleteCatalogCommand(catalogId));
            return ActionResult(result);
        }
    }
}