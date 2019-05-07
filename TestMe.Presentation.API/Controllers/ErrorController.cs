using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMe.SharedKernel.Domain;

namespace TestMe.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]   
    public class ErrorController : ControllerBase
    { 
        [AllowAnonymous]
        [HttpGet, HttpPost, HttpPut, HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]       
        [ProducesResponseType(StatusCodes.Status409Conflict)]       
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProblemDetails> HandleError()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var path = exceptionHandlerPathFeature?.Path;
            var error = exceptionHandlerPathFeature?.Error;

            if (error is DbUpdateConcurrencyException)
            {
                return Conflict();
            }

            if (error is DomainException domainException)
            {
                var expectedProblem = new ProblemDetails
                {
                    Title = "An expected domain error occurred!",
                    Status = StatusCodes.Status422UnprocessableEntity,
                    Detail = domainException.Message                              
                };
                expectedProblem.Extensions["traceId"] = HttpContext.TraceIdentifier;

                return StatusCode(StatusCodes.Status422UnprocessableEntity, expectedProblem);
            }          

            var unexpectedProblem = new ProblemDetails
            {
                Title = "An unexpected error occurred!",
                Status = StatusCodes.Status500InternalServerError,  
            };
            unexpectedProblem.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return StatusCode(StatusCodes.Status500InternalServerError, unexpectedProblem);
        }
    }    
}