using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestMe.BuildingBlocks.App;

namespace TestMe.Presentation.API
{
    [ApiController]
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
                        return StatusCode(StatusCodes.Status400BadRequest, CreateProblemDetails(ProblemDetailsType.ExpectedApp, error));
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
            long result = -1;

            if (HttpContext?.User?.Identity is ClaimsIdentity identity)
            {
                string? nameIdentifierValue = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Int64.TryParse(nameIdentifierValue, out result);
            }

            return result;
        }

        private protected enum ProblemDetailsType { ExpectedApp = 400, ExpectedDomain = 422, UnexpectedError = 500 }
        private protected ProblemDetails CreateProblemDetails(ProblemDetailsType type, string? detail = null)
        {
            var problemDetails = new ProblemDetails
            {               
                Detail = detail
            };

            switch (type)
            {
                case ProblemDetailsType.ExpectedApp:
                    problemDetails.Title = "An expected app error occurred!";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    break;
                case ProblemDetailsType.ExpectedDomain:
                    problemDetails.Title = "An expected domain error occurred!";
                    problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                    break;
                case ProblemDetailsType.UnexpectedError:
                    problemDetails.Title = "An unexpected error occurred!";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    break;
                default:
                    throw new NotImplementedException();
            }

            problemDetails.Extensions["traceId"] = HttpContext.TraceIdentifier;

            return problemDetails;
        }
    }
}
