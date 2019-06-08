using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Globalization;
using System.Diagnostics.Tracing;
using System.Runtime;
using TestMe.Presentation.API.Attributes;
using TestMe.SharedKernel.Domain;
using TestMe.SharedKernel.App;

namespace TestMe.Presentation.API.ControllersSpecial
{
    [ApiController]  
    public class LoaderController : Controller
    {
        [HttpGet("loaderio-33c0a1076f0b3616ddbc1280d9b8968f")] 
        public ActionResult LoaderIOVerification(long catalogId)
        {
            return Content("loaderio-33c0a1076f0b3616ddbc1280d9b8968f");
        }



        [HttpGet("loadTest/ThrowException")]
        public ActionResult ThrowException()
        {
            throw new DomainException(String.Empty);
        }

        [HttpGet("loadTest/ReturnResultError")]
        public ActionResult ReturnResultError()
        {
            var result = Result.Error(String.Empty);
            return ActionResult(result);
        }
    }
}
