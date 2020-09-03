using Shared.Configuration;
using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Shared.Helpers
{
    public class TelegramHelper
    {
        public static bool SendMessage(TelegramConfig telegramConfig, string toUserName, string message)
        {
            var tgClient = new TelegramBotClient(telegramConfig.ApiToken);
            var updates = tgClient.GetUpdatesAsync().GetAwaiter().GetResult();

            try
            {
                var id = updates
                    .Select(x => x.Message.Chat)
                    .First(x => x.Username != null && x.Username.Equals(toUserName, StringComparison.InvariantCultureIgnoreCase))
                    .Id;

                tgClient.SendTextMessageAsync(new ChatId(id), message).GetAwaiter().GetResult();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
