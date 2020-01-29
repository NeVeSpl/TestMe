using Microsoft.AspNetCore.Http;
using TestMe.UserManagement.App;

namespace TestMe.Presentation.API.Services
{
    internal sealed class CorrelationIdProvider : ICorrelationIdProvider
    {
        public string CorrelationId
        {
            get;
        }


        public CorrelationIdProvider(IHttpContextAccessor htppContextAccessor)
        {
            CorrelationId = htppContextAccessor.HttpContext.TraceIdentifier;
        }
    }
}