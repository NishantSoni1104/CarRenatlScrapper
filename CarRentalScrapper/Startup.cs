using System;
using System.Collections.Generic;
using System.Net;
using System.Transactions;
using CarRentalScrapper.Context;
using CarRentalScrapper.Database.IServices;
using CarRentalScrapper.Database.Services;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CarRentalScrapper
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;
            if (_hostingEnvironment.IsDevelopment())
            {
                connectionString = Configuration.GetConnectionString("Development");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("Production");
            }

            Console.WriteLine(connectionString);
            Console.WriteLine("Search");
            //    services.AddDbContextPool<ApplicationDbContext>(
            //        options =>
            //            options
            //                .UseMySql(connectionString,
            //                    mariaDbOptions =>
            //                    {
            //                        mariaDbOptions.ServerVersion(new Version(10, 2, 16), ServerType.MariaDb);
            //                    }
            //                )
            //                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //        );

            //    services.AddHangfire(x => x.UseStorage(new MySqlStorage(
            //connectionString,
            //new MySqlStorageOptions
            //{
            //    TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            //    QueuePollInterval = TimeSpan.FromSeconds(15),
            //    JobExpirationCheckInterval = TimeSpan.FromHours(1),
            //    CountersAggregateInterval = TimeSpan.FromMinutes(5),
            //    PrepareSchemaIfNecessary = true,
            //    DashboardJobListLimit = 50000,
            //    TransactionTimeout = TimeSpan.FromMinutes(1),
            //})));


            //MSSQL


            string conectionString = _hostingEnvironment.IsDevelopment() ?
                 Configuration.GetConnectionString("Development") :
                 Configuration.GetConnectionString("Production");
            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options
                .UseSqlServer(conectionString)
                .EnableSensitiveDataLogging();
            });
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(conectionString);
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

            services.AddScoped<IDbservice, Dbservice>();

            services.AddNodeServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext applicationDbContext)
        {
#if Debug
            app.UseDeveloperExceptionPage();
#else       
            app.UseExceptionHandler("/Home/Error");
#endif    

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseHangfireServer(new BackgroundJobServerOptions
            {
                HeartbeatInterval = new TimeSpan(0, 1, 0),
                ServerCheckInterval = new TimeSpan(0, 1, 0),
                SchedulePollingInterval = new TimeSpan(0, 1, 0)
            });

            var authFilter = new List<HangFireAuthorizationFilter>() { new HangFireAuthorizationFilter() };
            var option = new DashboardOptions
            {
                Authorization = authFilter,
            };
            app.UseHangfireDashboard("/hangfire", option);

            if (_hostingEnvironment.IsDevelopment())
            {
                RecurringJob.AddOrUpdate<BatchJob>("batch-id", p => p.Run(), Cron.Daily(23, 59),
             TimeZoneInfo.FindSystemTimeZoneById("Cen. Australia Standard Time"));
            }
            else
            {
                RecurringJob.AddOrUpdate<BatchJob>("batch-id", p => p.Run(), Cron.Daily(23, 59),
             TimeZoneConverter.TZConvert.GetTimeZoneInfo("Australia/Sydney"));
            }

        }

    }
}
