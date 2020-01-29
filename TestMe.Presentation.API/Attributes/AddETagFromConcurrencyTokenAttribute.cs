using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestMe.BuildingBlocks.App;

namespace TestMe.Presentation.API.Attributes
{
    /// <summary>
    /// If ConcurrencyToken is part of response payload then it is set also as response ETag header
    /// </summary>
    internal sealed class AddETagFromConcurrencyTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is IHaveConcurrencyToken valueWithToken)
                {
                    context.HttpContext.Response.Headers.Add("ETag", valueWithToken.ConcurrencyToken.ToString());
                }
            }
        }
    }
}
