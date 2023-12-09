using Crawler.Services.Configuration;
using Crawler.Services.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Services
{
    public static class CrawlerServicesExtensions
    {
        public static IServiceCollection AddCrawlerServices(
            this IServiceCollection services,
            EmailConfig emailConfig,
            TelegramConfig telegramConfig)
        {
            services.AddSingleton<EmailConfig>(emailConfig);
            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton<TelegramConfig>(telegramConfig);
            services.AddTransient<ITelegramService, TelegramService>();

            return services;
        }
    }
}
