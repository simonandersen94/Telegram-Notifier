using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramNofitier.Telegram.Interfaces;

namespace TelegramNofitier.Telegram
{
    public class TelegramReceiver {
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramSender _telegramSender;

        public TelegramReceiver(string botToken, ITelegramSender telegramSender) {
            _botClient = new TelegramBotClient(botToken);
            _telegramSender = telegramSender;
        }

        public void StartReceiving() {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cts.Token
            );

            Console.WriteLine("Telegram is listening for messages..");

        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken) {
            if (update.Message is not { } message)
                return;

            string text = message.Text;

            Console.WriteLine($"Received message: {text}");

            if (text == "_temp") {
                await _telegramSender.SendMessage("Temperatur i stuen: 9 grader");
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            Console.WriteLine($"Telegram Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
