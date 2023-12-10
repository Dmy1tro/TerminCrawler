using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crawler.App.Constants;
using Crawler.App.Models;
using Crawler.Core.Interfaces;
using Crawler.Core.Models;
using Crawler.Shared.Services;

namespace Crawler.App
{
    public class Worker
    {
        private readonly ICrawlerService _crawler;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;
        private readonly AppSettings _settings;
        private readonly CancellationToken _cancellationToken;

        public Worker(
            ICrawlerService crawler,
            IEmailService emailService,
            ITelegramService telegramService,
            AppSettings settings,
            CancellationToken cancellationToken)
        {
            _crawler = crawler;
            _emailService = emailService;
            _telegramService = telegramService;
            _settings = settings;
            _cancellationToken = cancellationToken;
        }

        public async Task Execute()
        {
            var errors = await ValidateMessaging();

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    DebugHelper.LogError(error);
                }

                throw new Exception("Failed to start.");
            }

            while (!_cancellationToken.IsCancellationRequested)
            {
                DebugHelper.LogInfo("Run search...");

                var searchResult = SearchResult.NotFound;

                try
                {
                    searchResult = await _crawler.Search();
                }
                catch (Exception ex)
                {
                    DebugHelper.LogError(ex.Message);
                }

                if (searchResult == SearchResult.Success)
                {
                    DebugHelper.LogSuccess("Info finded!");
                    DebugHelper.LogSuccess("Sending message...");

                    if (!string.IsNullOrEmpty(_settings.GeneralConfig.ToEmail))
                    {
                        await RunSafe(() =>
                        {
                            _emailService.SendEmail(
                                _settings.GeneralConfig.ToEmail,
                                MessageText.ObjectFindedSubject,
                                MessageText.ObjectFindedMessage(_settings.CrawlerConfig.Uri)
                            );

                            return Task.CompletedTask;
                        });
                    }

                    if (!string.IsNullOrEmpty(_settings.GeneralConfig.ToTelegram))
                    {
                        await RunSafe(() =>
                        {
                            return _telegramService.SendMessage(
                                _settings.GeneralConfig.ToTelegram,
                                MessageText.ObjectFindedMessage(_settings.CrawlerConfig.Uri)
                            );
                        });
                    }
                }
                else
                {
                    DebugHelper.LogInfo("Not found. Waiting...");
                }

                await Task.Delay(TimeSpan.FromMinutes(_settings.GeneralConfig.PeriodInMinutes));
            }
        }

        private async Task<ICollection<string>> ValidateMessaging(bool sendTestMessage = true)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(_settings.GeneralConfig.ToEmail.Trim()) &&
                string.IsNullOrEmpty(_settings.GeneralConfig.ToTelegram.Trim()))
            {
                errors.Add("Not found email or telegram info.\nPlease, input email or telegram or both information in appSettings.json file.");
                return errors;
            }

            if (!string.IsNullOrEmpty(_settings.GeneralConfig.ToEmail) && sendTestMessage)
            {
                try
                {
                    _emailService.SendEmail(_settings.GeneralConfig.ToEmail, MessageText.TestMessageSubject, MessageText.TestMailMessage);
                }
                catch (Exception ex)
                {
                    errors.Add($"Failed to send test message to email: {_settings.GeneralConfig.ToEmail}.");
                }
            }

            if (!string.IsNullOrEmpty(_settings.GeneralConfig.ToTelegram) && sendTestMessage)
            {
                try
                {
                    await _telegramService.SendMessage(_settings.GeneralConfig.ToTelegram, MessageText.TestTelegramMessage);
                }
                catch (Exception ex)
                {
                    errors.Add($"Failed to send test message to telegram: {_settings.GeneralConfig.ToTelegram}.");
                }
            }

            return errors;
        }

        private async Task RunSafe(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                DebugHelper.LogError(ex);
            }
        }
    }
}
