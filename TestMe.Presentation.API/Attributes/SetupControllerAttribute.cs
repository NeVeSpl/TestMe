using Microsoft.AspNetCore.Mvc.Filters;

namespace TestMe.Presentation.API
{
    /// <summary>
    /// It invokes controller.Setup(); before an action
    /// </summary>
    internal sealed class SetupControllerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is Controller controller)
            {
                controller.Setup();
            }
        }       
    }
}
