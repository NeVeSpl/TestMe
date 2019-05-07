using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestMe.SharedKernel.App;


namespace TestMe.Presentation.API
{
    public abstract class Controller : ControllerBase
    {
        protected long UserId;

        [NonAction]
        public void Setup()
        {
            UserId = GetAuthenticatedUserId();
        }


        protected ActionResult ActionResult(IResult result, bool resultOfCreatingResource = false)
        {
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    if (resultOfCreatingResource == false)
                    {
                        if (result.HasValue())
                        {
                            return Ok(result.GetValue());
                        }
                        else
                        {
                            return Ok();
                        }
                    }
                    else
                    {
                        if (result.HasValue())
                        {
                            return Created($"{GetControllerNameWithoutPostfix()}/{result.GetValue()}", result.GetValue());
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                case ResultStatus.Unauthorized:
                    return Unauthorized();
                case ResultStatus.Error:
                    string error = result.GetError();
                    if (String.IsNullOrEmpty(error))
                    {
                        return BadRequest();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, CreateProblemDetails(error));
                    }
                case ResultStatus.NotFound:
                    return NotFound();
                case ResultStatus.Conflict:
                    return Conflict();
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetControllerNameWithoutPostfix()
        {            
            string controllerFullName = this.GetType().Name;
            return controllerFullName.Substring(0, controllerFullName.Length - "Controller".Length);
        }
        private long GetAuthenticatedUserId()
        {
            var identity = HttpContext?.User?.Identity as ClaimsIdentity;
            long result = -1;

            if (identity != null)
            {
                string nameIdentifierValue = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Int64.TryParse(nameIdentifierValue, out result);
            }

            return result;
        }
        private ProblemDetails CreateProblemDetails(string error)
        {
            var problemDetails = new ProblemDetails
            {
                Title = "An expected app error occurred!",
                Status = StatusCodes.Status400BadRequest,
                Detail = error
            };
            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return problemDetails;
        }
    }
}
