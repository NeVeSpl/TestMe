using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TestMe.TestCreation.App.Ports;

namespace TestMe.Presentation.API.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public long UserId
        {
            get;
        }


        public UserIdProvider(IHttpContextAccessor htppContextAccessor)
        {
            UserId = GetAuthenticatedUserId(htppContextAccessor.HttpContext);
        }


        public static long GetAuthenticatedUserId(HttpContext? htppContext)
        {
            long result = -1;

            if (htppContext?.User?.Identity is ClaimsIdentity identity)
            {
                string? nameIdentifierValue = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Int64.TryParse(nameIdentifierValue, out result);
            }

            return result;
        }
    }
}
