using Shared.Configuration;
using Shared.Helpers;
using System;
using System.Threading.Tasks;
using WebCrawler.Constants;
using WebCrawler.Interfaces;
using WebCrawler.Models;

namespace WebCrawler
{
    public class Worker
    {
        private readonly ITerminChecker _terminChecker;
        private readonly GeneralConfig _config;
        private readonly EmailConfig _emailConfig;
        private readonly TelegramConfig _telegramConfig;

        public Worker(ITerminChecker terminChecker, Settings settings)
        {
            _terminChecker = terminChecker;
            _config = settings.GeneralConfig;
            _emailConfig = settings.EmailConfig;
            _telegramConfig = settings.TelegramConfig;
        }

        public void Execute()
        {
            if (CheckMessagingWorks() == false)
            {
                throw new Exception("Watch previous error.");
            }

            while (true)
            {
                DebugHelper.LogInfo("Search termin.");

                var hasTermin = false;

                try
                {
                    hasTermin = _terminChecker.HasTermin();
                }
                catch (Exception ex)
                {
                    DebugHelper.LogError(ex.Message);
                }

                if (hasTermin)
                {
                    DebugHelper.LogSuccess("Termin finded!");
                    DebugHelper.LogInfo($"Sending info...");

                    if (!string.IsNullOrEmpty(_config.ToEmail))
                    {
                        EmailHelper.SendEmail(_emailConfig,
                                              _config.ToEmail, 
                                              MessageText.TerminFindedSubject, 
                                              MessageText.TerminFindedMessage(_config.TerminUri));
                    }

                    if (!string.IsNullOrEmpty(_config.ToTelegram))
                    {
                        TelegramHelper.SendMessage(_telegramConfig, 
                                                   _config.ToTelegram,
                                                   MessageText.TerminFindedMessage(_config.TerminUri));
                    }
                }
                else
                {
                    DebugHelper.LogInfo("Termin not finded.");
                }

                Task.Delay(TimeSpan.FromMinutes(_config.PeriodInMinutes)).Wait();
            }
        }

        private bool CheckMessagingWorks()
        {
            var isValid = true;

            if (string.IsNullOrEmpty(_config.ToEmail.Trim()) &&
                string.IsNullOrEmpty(_config.ToTelegram.Trim()))
            {
                DebugHelper.LogError("Not found email or telegram info.\n" +
                                     "Please, input needed information in appSettings.json file.");

                isValid = false;
            }

            if (!string.IsNullOrEmpty(_config.ToEmail))
            {
                var emailWorks = EmailHelper.SendEmail(_emailConfig, 
                                                       _config.ToEmail, 
                                                       MessageText.TestMessageSubject, 
                                                       MessageText.TestMailMessage);

                if (emailWorks == false)
                {
                    DebugHelper.LogError($"Failed to send test message to email: {_config.ToEmail}.");

                    isValid = false;
                }
            }

            if (!string.IsNullOrEmpty(_config.ToTelegram))
            {
                var telegaWorks = TelegramHelper.SendMessage(_telegramConfig, 
                                                             _config.ToTelegram, 
                                                             MessageText.TestTelegramMessage);

                if (telegaWorks == false)
                {
                    DebugHelper.LogError($"Failed to send test message to telegram: {_config.ToTelegram}.");

                    isValid = false;
                }
            }

            return isValid;
        }
    }
}
