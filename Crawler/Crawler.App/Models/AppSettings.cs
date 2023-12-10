using Crawler.Anticaptcha.Configs;
using Crawler.Core.Configs;
using Crawler.Services.Configuration;
using Crawler.Shared.Configuration;

namespace Crawler.App.Models
{
    public class AppSettings
    {
        public AppConfig GeneralConfig { get; set; }

        public CrawlerConfig CrawlerConfig { get; set; }

        public EmailConfig EmailConfig { get; set; }

        public TelegramConfig TelegramConfig { get; set; }

        public AnticaptchaConfig AnticaptchaConfig { get; set; }
    }
}
