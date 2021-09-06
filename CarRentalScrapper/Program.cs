using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.Globalization;
using System.Net;
using WebApp;

namespace CarRentalScrapper
{
    public class Program
    {
        public static int Main(string[] args)
        {
          

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateWebHostBuilder(args)
                    .Build()
                    .Seed()
                    .Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("config.json");
                })
                //.ConfigureKestrel(options =>
                //{
                //    options.Listen(IPAddress.Loopback, 5005); //HTTP port
                //})
                //.UseUrls("http://localhost:5005")
                //.UseSerilog()
                //.ConfigureKestrel(options =>
                //{
                //    options.Listen(IPAddress.Loopback, 5001);
                //    //options.Listen(IPAddress.Any, 84, listenOptions =>
                //    //{
                //    //    listenOptions.UseHttps("certificate.pfx", "password");
                //    //});
                //})

                .UseStartup<Startup>();
    }
}
