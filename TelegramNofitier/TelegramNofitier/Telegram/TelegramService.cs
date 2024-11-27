using System;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramNofitier.Config;

namespace TelegramNofitier.Telegram {
    public class TelegramService {
        private readonly TelegramBotClient _botClient;
        private readonly Config.Config _config;

        public TelegramService(Config.Config config) {
            _config = config;
            var botToken = _config.TelegramBotToken;
            _botClient = new TelegramBotClient(botToken);
        }

        public async Task SendMessageAsync(string message) {
            try {
                var chatId = _config.ChatID;
                var sentMessage = await _botClient.SendMessage(chatId: chatId, text: message);
                Console.WriteLine($"Besked sendt til chatId: {chatId} med messageId: {sentMessage.MessageId}");
            } catch (Exception ex) {
                Console.WriteLine($"Fejl ved afsendelse af besked: {ex.Message}");
            }
        }
    }
}
