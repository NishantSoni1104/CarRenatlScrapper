using CarRentalScrapper.Context;
using CarRentalScrapper.Database.IServices;
using CarRentalScrapper.Models;
using CarRentalScrapper.ViewModels;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentalScrapper
{
    public class BatchJob
    {
        private readonly ApplicationDbContext _context;
        private readonly INodeServices nodeService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDbservice _dbservice;
        private MailHelper mailHelper;
        private static Logger logger;
        public BatchJob(INodeServices nodeService, IHostingEnvironment hostingEnvironment, IDbservice dbservice, ApplicationDbContext context)
        {
            _context = context;
            this.nodeService = nodeService;
            _hostingEnvironment = hostingEnvironment;
            _dbservice = dbservice;
            logger = LogManager.GetCurrentClassLogger();
            mailHelper = new MailHelper();
        }

        [AutomaticRetry(Attempts = 0)]
        public async Task Run()
        {
          try
            {
      
                List<PriceHistoryDto> jobs = new List<PriceHistoryDto>();

                foreach (var setting in await _dbservice.AllSettings())
                {

                    try
                    {
                        if (await _dbservice.IsScrapeProcessExecuted(setting))
                        {
                            var scrapeProcessId = await _dbservice.AddScrapeProcesses(setting);
                            var results = await nodeService.InvokeAsync<List<PriceHistoryDto>>("./NodeServices/app",
                                new
                                {
                                    location = setting.URL,
                                    pickupDate = setting.PickupDateInterval,
                                    returnDate = setting.ReturnDateInterval,
                                    id = setting.Id,
                                    sPiD = scrapeProcessId
                                });



                            
                            foreach (var result in results)
                                result.ScrapeProcessId = scrapeProcessId;

                            await _dbservice.AddPriceHistories(results);
                            if (results.Count > 0)
                            {
                                jobs.AddRange(results);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        logger.Debug(ex.Message);
                    }

                }

                takeScreenShot(jobs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                logger.Debug(ex.Message);
                        }
        }



         

        private async void takeScreenShot(List<PriceHistoryDto> listDto)
        {
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            contentRootPath = Path.Combine(contentRootPath, "ScreenShot");
            string folderName = DateTime.Now.ToString("MM/dd/yyyy").Replace('/', '-');
            string path = Path.Combine(contentRootPath, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            HTMLToPNG hTMLToPNG = new HTMLToPNG();
            foreach (var item in listDto)
            {
                try
                {
                    

                    hTMLToPNG.TakingHTML2CanvasFullPageScreenshot(item.detailUrl,path+ "/" + item.vehicle + item.ScrapeProcessId + ".png");
                }
                catch (Exception ex)
                {
                    logger.Debug(ex.Message);

                }
            }
        }
    }
    public class MailHelper
    {
        private static Logger logger;
        private string mailFrom = "statelawyerssydney@gmail.com";
        private string username = "statelawyerssydney@gmail.com";
        private string password = "julliathroth";
        private string mailTo = "atlantistrash@hotmail.com,nishh.soni@gmail.com";
        private string subject = "An exception occured in Application - Car Rental Scrapper.";
        private string mailBody = "";
        private string senderName = "Car Rental Scrapper.";
        public MailHelper()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public void Send(string errMsg, string errLocation)
        {
            try
            {
                //create Mail Body
                mailBody = "Dear Team," +
                            Environment.NewLine + "An exception occurred in a Application Car Rental Scrapper." + Environment.NewLine +
                            "  Log Written Date:  " + " " + DateTime.Now.ToString() +
                            Environment.NewLine + "   Error Line No  " + " " + errLocation + "\t\n" +
                           Environment.NewLine + " Error Message:  " + " " + errMsg + Environment.NewLine +
                            "Thanks and Regards," + Environment.NewLine + senderName;




                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(mailFrom);
                mail.To.Add(mailTo);
                mail.Subject = subject;
                mail.Body = mailBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {
                logger.Debug(ex.Message);
            }
        }
    }

}
