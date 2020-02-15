using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace TestMe.Presentation.API.Middleware
{
    /// <summary>
    /// It is sample showing how to use custom Middleware rather than something that fulfils a real need in this project.
    /// </summary>
    public sealed class RequestLogger
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLogger> logger;

        public RequestLogger(RequestDelegate next, ILogger<RequestLogger> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await next.Invoke(context);

            var scopeData = new Dictionary<string, object>
            {
                { "RequestMethod", context.Request.Method },
                { "RequestQuery", context.Request.Query},
                { "RequestQueryString", context.Request.QueryString },
                { "RequestPath", context.Request.Path },
                { "RequestPathBase", context.Request.PathBase },              
                { "RequestContentType", context.Request.ContentType },
                { "StatusCode", context.Response.StatusCode }
            };

            stopwatch.Stop();
            using (var scope = logger.BeginScope(scopeData))
            {
                logger.LogInformation(1000, "Request completed in {ElapsedTime} ms ", stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
