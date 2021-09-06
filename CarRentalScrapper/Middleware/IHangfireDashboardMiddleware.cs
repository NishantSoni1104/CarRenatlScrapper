using CarRentalScrapper.MiddleWare;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalScrapper.Middleware
{
    public interface IHangfireDashboardMiddleware 
    {
        Task Invoke(HttpContext httpContext);
    }
}
