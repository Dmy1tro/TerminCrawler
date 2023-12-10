using System;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Shared.Configuration;
using Telegram.Bot;

namespace Crawler.Shared.Services
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
                    .FirstOrDefault(x => x != null && x.Username != null && x.Username.Equals(toUserName, StringComparison.InvariantCultureIgnoreCase))?
                    .Id;

            if (chatId == null)
            {
                throw new Exception($"Failed to found telegram chat with user '${toUserName}'. Make sure you /start the bot.");
            }

            await tgClient.SendTextMessageAsync(chatId, message);
        }
    }
}
