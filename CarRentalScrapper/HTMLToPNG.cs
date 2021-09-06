using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
namespace CarRentalScrapper
{
    public class HTMLToPNG
    {
        public void TakingHTML2CanvasFullPageScreenshot(string URL,string filePath)
        {
            
                ChromeDriver driver = new ChromeDriver();
                string page_to_screenshot = URL;
                driver.Url = page_to_screenshot;
                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(25));
                System.Threading.Thread.Sleep(9000);
                //Dictionary will contain the parameters needed to get the full page screen shot
                Dictionary<string, Object> metrics = new Dictionary<string, Object>();
                metrics["width"] = driver.ExecuteScript("return Math.max(window.innerWidth,document.body.scrollWidth,document.documentElement.scrollWidth)");
                metrics["height"] = driver.ExecuteScript("return Math.max(window.innerHeight,document.body.scrollHeight,document.documentElement.scrollHeight)");
                metrics["deviceScaleFactor"] = Convert.ToDouble(driver.ExecuteScript("return window.devicePixelRatio"));
                metrics["mobile"] = driver.ExecuteScript("return typeof window.orientation !== 'undefined'");
                //Execute the emulation Chrome Command to change browser to a custom device that is the size of the entire page
                driver.ExecuteChromeCommand("Emulation.setDeviceMetricsOverride", metrics);
                //You can then just screenshot it as it thinks everything is visible
                string path_to_save_screenshot = filePath;
                driver.GetScreenshot().SaveAsFile(path_to_save_screenshot, ScreenshotImageFormat.Png);
                //This command will return your browser back to a normal, usable form if you need to do anything else with it.
                driver.ExecuteChromeCommand("Emulation.clearDeviceMetricsOverride", new Dictionary<string, Object>());
                driver.Close();

                //Process[] AllProcesses = Process.GetProcesses() ;
                //foreach (var process in AllProcesses)
                //{
                //    if (process.MainWindowTitle != "")
                //    {
                //        string s = process.ProcessName.ToLower();
                //        if (s == "iexplore" || s == "iexplorer" || s == "chrome" || s == "firefox")
                //            process.Kill();
                //    }
                //}
            
        }
        public static bool WaitForPageLoad(IWebDriver driver, TimeSpan waitTime)
        {
            WaitForDocumentReady(driver, waitTime);
            bool ajaxReady = WaitForAjaxReady(driver, waitTime);
            WaitForDocumentReady(driver, waitTime);
            return ajaxReady;
        }
        private static void WaitForDocumentReady(IWebDriver driver, TimeSpan waitTime)
        {
            var wait = new WebDriverWait(driver, waitTime);
            var javascript = driver as IJavaScriptExecutor;
            if (javascript == null)
                throw new ArgumentException("driver", "Driver must support javascript execution");

            wait.Until((d) =>
            {
                try
                {
                    string readyState = javascript.ExecuteScript(
                        "if (document.readyState) return document.readyState;").ToString();
                    return readyState.ToLower() == "complete";
                }
                catch (InvalidOperationException e)
                {
                    return e.Message.ToLower().Contains("unable to get browser");
                }
                catch (WebDriverException e)
                {
                    return e.Message.ToLower().Contains("unable to connect");
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
        private static bool WaitForAjaxReady(IWebDriver driver, TimeSpan waitTime)
        {
            System.Threading.Thread.Sleep(1000);
            WebDriverWait wait = new WebDriverWait(driver, waitTime);
            return wait.Until<bool>((d) =>
            {
                return driver.FindElements(By.CssSelector(".waiting, .tb-loading")).Count == 0;
            });
        }


    }
}
