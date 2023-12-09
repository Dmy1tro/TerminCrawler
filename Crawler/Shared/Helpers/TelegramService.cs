using System;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Services.Configuration;
using Telegram.Bot;

namespace Crawler.Services.Helpers
{
    public interface ITelegramService
    {
        Task SendMessage(string toUserName, string message);
    }

    public class TelegramService : ITelegramService
    {
        private readonly TelegramConfig _telegramConfig;

        public TelegramService(TelegramConfig telegramConfig)
        {
            _telegramConfig = telegramConfig;
        }

        public async Task SendMessage(string toUserName, string message)
        {
            var tgClient = new TelegramBotClient(_telegramConfig.ApiToken);
            var updates = await tgClient.GetUpdatesAsync();

            var chatId = updates
                    .Select(x => x.Message?.Chat)
                    .FirstOrDefault(x => x != null && x.Username != null && x.Username.Equals(toUserName, StringComparison.InvariantCultureIgnoreCase))
                    .Id;

            await tgClient.SendTextMessageAsync(chatId, message);
        }
    }
}
