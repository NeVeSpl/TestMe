using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMe.BuildingBlocks.Domain;

namespace TestMe.Presentation.API.ControllersSpecial
{

    [Route("[controller]")]
    public class ErrorController : Controller
    {
        /// <summary>
        /// A special endpoint that acts as ExceptionHandler for uncatched exceptions
        /// </summary>       
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
                var expectedProblem = CreateProblemDetails(ProblemDetailsType.ExpectedDomain, domainException.Message); 
                return StatusCode(StatusCodes.Status422UnprocessableEntity, expectedProblem);
            }

            var unexpectedProblem = CreateProblemDetails(ProblemDetailsType.UnexpectedError);
            return StatusCode(StatusCodes.Status500InternalServerError, unexpectedProblem);
        }


        //public enum ErrorType { Conflict }

        //[AllowAnonymous]
        //[HttpGet]
        //[LocalHostOnly]
        //[ProducesResponseType(StatusCodes.Status409Conflict)]
        //public ActionResult<ProblemDetails> ThrowMe(ErrorType errorType)
        //{
        //    return errorType switch
        //    {
        //        ErrorType.Conflict => Conflict(),
        //    };         
        //}
    }
}