using Microsoft.AspNetCore.Mvc.Filters;

namespace TestMe.Presentation.API
{
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
