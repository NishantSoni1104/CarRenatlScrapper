using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CarRentalScrapper.Context;
using CarRentalScrapper.Models;
using CarRentalScrapper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Pomelo.EntityFrameworkCore.MySql;

namespace CarRentalScrapper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private MailHelper mailHelper;
        private static Logger logger;
   
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            logger = LogManager.GetCurrentClassLogger();
            mailHelper = new MailHelper();

       
        }

       

        private void getData()
        {
            string connetionString = null;
            SqlConnection sqlCnn;
            SqlCommand sqlCmd;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            int i = 0;
            string sql = null;

            connetionString = "Server=MACHINE\\SQLEXPRESS; Database= carscl5_carscraper; Integrated Security = true";
            sql = "Select * from Pricehistories";

            List<PriceHistory> priceHistories = new List<PriceHistory>();
            sqlCnn = new SqlConnection(connetionString);
            try
            {
                sqlCnn.Open();
                sqlCmd = new SqlCommand(sql, sqlCnn);
                adapter.SelectCommand = sqlCmd;
                adapter.Fill(ds);
                priceHistories = ConvertDataTable<PriceHistory>(ds.Tables[0]);
                adapter.Dispose();
                sqlCmd.Dispose();
                sqlCnn.Close();
            }
            catch (Exception ex)
            {
                
            }
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                getData();
                    return View(await _context.PriceHistories
                    .AsNoTracking()
                    .ToListAsync());
            }
            catch (System.Exception ex)
            {
                logger.Debug(ex.Message);
               // mailHelper.Send(ex.Message, "HomeController@Index");
                return Json(ex.Message);
            }
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    try
                    {
                        if (pro.Name == column.ColumnName)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            return obj;
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SettingPost(Settingsdto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var settings = new Setting
                    {
                        PickupDate = model.PickupDateInterval,
                        PickupLocation = model.PickupLocation,
                        ReturnDate = model.ReturnDateInterval,
                        Country = "Australia",
                        DriverAge = "30 - 69"
                    };

                    await _context.Settings.AddAsync(settings);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GetSetting));
                }
                return View();
            }
            catch (System.Exception ex)
            {

                logger.Debug(ex.Message);
                mailHelper.Send(ex.Message, "HomeController@SettingPost");
                return Json(ex.Message);
            }
        }

        public async Task <IActionResult> Setting(int id)
        {
            try
            {
                var setting = await _context.Settings
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new Settingsdto
                {
                    Id = i.Id,
                    PickupLocation = i.PickupLocation,
                    PickupDateInterval = i.PickupDate,
                    ReturnDateInterval = i.ReturnDate
                })
                 .SingleOrDefaultAsync();
                return View(setting);
            }
            catch (System.Exception ex)
            {
                logger.Debug(ex.Message);
                mailHelper.Send(ex.Message, "HomeController@Setting");
                return Json(ex.Message);
            }
        }


        public async Task<IActionResult> UpdateSetting(Settingsdto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var setting = new Setting { Id = model.Id };
                    _context.Attach(setting);
                    setting.PickupDate = model.PickupDateInterval;
                    setting.ReturnDate = model.ReturnDateInterval;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GetSetting));
                }
                return View(nameof(Setting));
            }
            catch (System.Exception ex)
            {
                logger.Debug(ex.Message);
                mailHelper.Send(ex.Message, "HomeController@UpdateSetting");
                return Json(ex.Message);
            }
        }

        private void GetData()
        {
            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = "Server=MACHINE\\SQLEXPRESS; Database= carscl5_carscraper; User Id=ssa;Password=Enterpreneur@1104;";
            //SqlDataAdapter da = new SqlDataAdapter();
            //SqlCommand sqlCommand= new SqlCommand("Select * from PriceHistories0");
            //da.Fill(sqlCommand,con)

        }

        public async Task<IActionResult> GetSetting()
        {
            try
            {
                var setting = await _context.Settings
                     .AsNoTracking()
                     .Select(i => new SettingList
                     {
                         Id = i.Id,
                         PickupLocation = i.PickupLocation,
                         PickupDate = i.PickupDate,
                         ReturnDate = i.ReturnDate

                     })
                      .ToListAsync();
                var model = new Settingsdto
                {
                    GridSetting = setting
                };
                return View(model);
            }
            catch (System.Exception ex)
            {
                logger.Debug(ex.Message);
                mailHelper.Send(ex.Message, "HomeController@GetSetting");
                return Json(ex.Message);
            }
        }
    }
}
