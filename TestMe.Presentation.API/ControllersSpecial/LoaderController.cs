using System;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;
using TestMe.BuildingBlocks.Domain;

namespace TestMe.Presentation.API.ControllersSpecial
{  
    public class LoaderController : Controller
    {
        /// <summary>
        /// A special endpoint that verify ownership of server for loader.io
        /// </summary>        
        [HttpGet("loaderio-33c0a1076f0b3616ddbc1280d9b8968f")] 
        public ActionResult LoaderIOVerification()
        {
            return Content("loaderio-33c0a1076f0b3616ddbc1280d9b8968f");
        }
                
        [HttpGet("loadTest/ThrowException")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult ThrowException()
        {
            throw new DomainException(String.Empty);
        }

        [HttpGet("loadTest/ReturnResultError")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult ReturnResultError()
        {
            var result = Result.Error(String.Empty);
            return ActionResult(result);
        }
    }
}