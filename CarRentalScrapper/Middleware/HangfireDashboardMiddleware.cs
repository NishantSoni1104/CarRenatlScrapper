using CarRentalScrapper.Middleware;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CarRentalScrapper.MiddleWare
{
    public class HangfireDashboardMiddleware : IHangfireDashboardMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JobStorage _storage;
        private readonly DashboardOptions _options;
        private readonly RouteCollection _routes;

        public HangfireDashboardMiddleware(RequestDelegate next, JobStorage storage, DashboardOptions options, RouteCollection routes)
        {
            _next = next;
            _storage = storage;
            _options = options;
            _routes = routes;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var findResult = _routes.FindDispatcher(httpContext.Request.Path.Value);
            if (findResult == null)
            {
                await _next.Invoke(httpContext);
                return;
            }

            // attempt to authenticate against default auth scheme (this will attempt to authenticate using data in request, but doesn't send challenge)
            var result = await httpContext.AuthenticateAsync();
            if (!result.Succeeded)
            {
                // request was not authenticated, send challenge and do not continue processing this request
                await httpContext.ChallengeAsync();
                return;
            }

            var aspNetCoreDashboardContext = new AspNetCoreDashboardContext(_storage, _options, httpContext);

            foreach (var filter in _options.Authorization)
            {
                if (filter.Authorize(aspNetCoreDashboardContext) == false)
                {
                    var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated;
                    httpContext.Response.StatusCode = isAuthenticated == true ? (int)HttpStatusCode.Forbidden : (int)HttpStatusCode.Unauthorized;
                    return;
                }
            }

            aspNetCoreDashboardContext.UriMatch = findResult.Item2;
            await findResult.Item1.Dispatch(aspNetCoreDashboardContext);
        }
    }
}
