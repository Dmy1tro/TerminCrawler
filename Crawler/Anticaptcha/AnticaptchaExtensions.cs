using Crawler.Anticaptcha.Configs;
using Crawler.Anticaptcha.Interfaces;
using Crawler.Anticaptcha.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.Anticaptcha
{
    public static class AnticaptchaExtensions
    {
        public static IServiceCollection AddAnticaptcha(this IServiceCollection services, AnticaptchaConfig config)
        {
            // Better to use IOptions.
            services.AddSingleton<AnticaptchaConfig>(config);
            services.AddSingleton<IAnticaptchaService, AnticaptchaService>();
            services.AddHttpClient("anticaptcha_client");

            return services;
        }
    }
}
