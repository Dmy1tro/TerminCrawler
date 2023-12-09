using Crawler.Core.Configs;
using Crawler.Core.Interfaces;
using Crawler.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;

namespace Crawler.Core
{
    public static class CrawlerCoreExtensions
    {
        public static IServiceCollection AddCrawlerCore(
            this IServiceCollection services,
            CrawlerConfig config,
            ChromeDriver chromeDriver)
        {
            services.AddSingleton<CrawlerConfig>(config);
            services.AddSingleton<ChromeDriver>(chromeDriver);
            services.AddTransient<ICrawlerService, TerminChecker>();

            return services;
        }
    }
}
