using Anticaptcha.Interfaces;
using Anticaptcha.Services;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Shared.Helpers;
using System;
using System.IO;
using WebCrawler.Models;
using WebCrawler.Services;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = GetSettings();

            var anticaptchaService = (IAnticaptchaService)new AnticaptchaService(settings.AnticaptchaConfig);
            var chromeDriver = (RemoteWebDriver)new ChromeDriver();
            var terminChecker = new TerminChecker(chromeDriver, anticaptchaService, settings.GeneralConfig.TerminUri);

            var worker = new Worker(terminChecker, settings);

            try
            {
                worker.Execute();
            }
            catch (Exception ex)
            {
                DebugHelper.LogError(ex.Message);
            }
            finally
            {
                chromeDriver.Quit();
            }

            DebugHelper.LogInfo("Press any key to exit.");

            Console.ReadKey();
        }

        private static Settings GetSettings()
        {
            var json = File.ReadAllText("appSettings.json");

            var settings = JsonConvert.DeserializeObject<Settings>(json);

            return settings;
        }
    }
}
