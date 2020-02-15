using Microsoft.AspNetCore.Http;
using TestMe.UserManagement.App;

namespace TestMe.Presentation.API.Services
{
    internal sealed class TraceIdProvider : ITraceIdProvider
    {
        public string TraceId
        {
            get;
        }


        public TraceIdProvider(IHttpContextAccessor htppContextAccessor)
        {
            TraceId = htppContextAccessor.HttpContext.TraceIdentifier;
        }
    }
}