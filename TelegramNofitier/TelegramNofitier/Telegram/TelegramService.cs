using System;
using Telegram.Bot;
using TelegramNofitier.Config;

namespace TelegramNofitier.Telegram {
    public class TelegramService {
        private readonly Config.Config _config;

        public TelegramService(Config.Config config) {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public TelegramBotClient CreateBotClient() {
            var botToken = _config.TelegramBotToken;
            return new TelegramBotClient(botToken);
        }
    }
}
