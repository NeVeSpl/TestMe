using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestMe.SharedKernel.App;

namespace TestMe.Presentation.API.Attributes
{
    internal sealed class AddETagFromConcurrencyTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.Value is IHasConcurrencyToken valueWithToken)
                {
                    context.HttpContext.Response.Headers.Add("ETag", valueWithToken.ConcurrencyToken.ToString());
                }
            }
        }
    }
}
