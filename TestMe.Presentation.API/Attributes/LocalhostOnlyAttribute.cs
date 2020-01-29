using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestMe.Presentation.API.Attributes
{
    /// <summary>
    /// It allows accessing to endpoint only if a request comes from localhost
    /// </summary>
    internal sealed class LocalHostOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            bool isLocal = false;

            var remoteAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

            if (remoteAddress == "127.0.0.1" || remoteAddress == "::1")
            {
                isLocal = true;
            }
          
            if (remoteAddress == context.HttpContext.Connection.LocalIpAddress.ToString())
            {
                isLocal = true;
            }

            if (!isLocal)
            {
                context.Result = new NotFoundResult();
            }     
        }
    }
}
