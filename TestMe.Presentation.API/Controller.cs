using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TestMe.BuildingBlocks.App;
using TestMe.Presentation.API.Services;
using TestMe.TestCreation.App.RequestEnrichers;

namespace TestMe.Presentation.API
{
    [ApiController]
    public abstract class Controller : ControllerBase
    {
        protected long UserId;    
       

        [NonAction]
        public void Setup()
        {
            UserId = UserIdProvider.GetAuthenticatedUserId(HttpContext);
        }


        private protected Task<T> Send<T>(IRequest<T> request)
        {
            if (@request is IHaveUserId iHaveUserId) // if we will have more things to do in pipeline moving to MediatR behaviors
            {                                        // or decorators would be better solution, but now KISS
                iHaveUserId.UserId = UserId;
            }
            var mediatr = HttpContext.RequestServices.GetRequiredService<IMediator>();
            return mediatr.Send(request);
        }
      
       
        protected ActionResult<T> ActionResult<T>(Result<T> result, bool resultOfCreatingResource = false)
        {
            return ActionResultImplementation(result, resultOfCreatingResource);
        }
        protected ActionResult ActionResult(Result result, bool resultOfCreatingResource = false)
        {
            return ActionResultImplementation(result, resultOfCreatingResource);
        }

        private ActionResult ActionResultImplementation(IResult result, bool resultOfCreatingResource = false)
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