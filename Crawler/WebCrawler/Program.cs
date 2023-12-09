using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Crawler.Anticaptcha;
using Crawler.App.Models;
using Crawler.Core;
using Crawler.Core.Interfaces;
using Crawler.Services;
using Crawler.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;

namespace Crawler.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = BuildServiceProvider();

            var settings = provider.GetRequiredService<AppSettings>();
            var chromeDriver = provider.GetRequiredService<ChromeDriver>();
            var crawler = provider.GetRequiredService<ICrawlerService>();
            var emailService = provider.GetRequiredService<IEmailService>();
            var telegramService = provider.GetRequiredService<ITelegramService>();
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, e) =>
            {
                cts.Cancel();
                e.Cancel = true;
            };

            var worker = new Worker(crawler, emailService, telegramService, settings, cts.Token);

            try
            {
                await worker.Execute();
            }
            catch (Exception ex)
            {
                DebugHelper.LogError(ex.Message);
            }
            finally
            {
                chromeDriver.Quit();
            }

            Console.ReadKey();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var settings = GetAppSettings();
            var services = new ServiceCollection();
            var chromeDriver = new ChromeDriver();

            services.AddSingleton<AppSettings>(settings);
            services.AddAnticaptcha(settings.AnticaptchaConfig);
            services.AddCrawlerServices(settings.EmailConfig, settings.TelegramConfig);
            services.AddCrawlerCore(settings.CrawlerConfig, chromeDriver);

            var provider = services.BuildServiceProvider();

            return provider;
        }

        private static AppSettings GetAppSettings()
        {
            var json = File.ReadAllText("appSettings.json");

            var settings = JsonConvert.DeserializeObject<AppSettings>(json);

            return settings;
        }
    }
}
