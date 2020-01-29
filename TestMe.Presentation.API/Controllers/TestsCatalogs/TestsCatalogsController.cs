using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestMe.Presentation.API.Controllers.TestsCatalogs.Input;
using TestMe.TestCreation.App.Catalogs;
using TestMe.TestCreation.App.Catalogs.Input;
using TestMe.TestCreation.App.Catalogs.Output;

namespace TestMe.Presentation.API.Controllers.TestsCatalogs
{
    [Route("[controller]")]
    [Authorize]
    [SetupController]
    [ApiConventionType(typeof(ApiConventions))]
    public class TestsCatalogsController : Controller
    {
        private readonly ITestsCatalogsService service;


        public TestsCatalogsController(ITestsCatalogsService service)
        {
            this.service = service;
        }
        

        [HttpGet("headers")]
        public ActionResult<List<CatalogHeaderDTO>> ReadCatalogHeaders()
        {
            var result = service.ReadCatalogHeaders(UserId);
            return ActionResult(result);
        }

        [HttpGet("{catalogId}")]
        public ActionResult<CatalogDTO> ReadTestsCatalog(long catalogId)
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