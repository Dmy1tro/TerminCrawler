using Crawler.Shared.Configuration;
using Crawler.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Shared
{
    public static class CrawlerSharedExtensions
    {
        public static IServiceCollection AddCrawlerShared(
            this IServiceCollection services,
            EmailConfig emailConfig,
            TelegramConfig telegramConfig)
        {
            services.AddSingleton(emailConfig);
            services.AddTransient<IEmailService, EmailService>();

            services.AddSingleton(telegramConfig);
            services.AddTransient<ITelegramService, TelegramService>();

            return services;
        }
    }
}
