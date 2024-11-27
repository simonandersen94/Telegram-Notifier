using System;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramNofitier.Config;
using TelegramNofitier.Telegram.Interfaces;

namespace TelegramNofitier.Telegram {
    public class TelegramSender : ITelegramSender {
        private readonly TelegramBotClient _botClient;
        private readonly string _chatId;

        public TelegramSender(TelegramBotClient botClient, string chatId) {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _chatId = chatId;
        }

        public async Task SendMessage(string message) {
            try {
                var sentMessage = await _botClient.SendMessage(chatId: _chatId, text: message);
                Console.WriteLine($"Besked sendt til chatId: {_chatId} med messageId: {sentMessage.MessageId}");
            } catch (Exception ex) {
                Console.WriteLine($"Fejl ved afsendelse af besked: {ex.Message}");
            }
        }
    }
}
