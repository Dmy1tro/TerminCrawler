using Anticaptcha.Configs;
using Shared.Configuration;

namespace WebCrawler.Models
{
    public class Settings
    {
        public GeneralConfig GeneralConfig { get; set; }

        public EmailConfig EmailConfig { get; set; }

        public TelegramConfig TelegramConfig{ get; set; }

        public AnticaptchaConfig AnticaptchaConfig { get; set; }
    }
}
