using TelegramNofitier.RabbitMQ;
using TelegramNofitier.Config;
using System;
using System.Threading.Tasks;
using TelegramNofitier.Telegram;
using TelegramNofitier.Telegram.Interfaces;
using TelegramNofitier.RabbitMQ.Interfaces;

namespace TelegramNofitier {
    internal class Program {
        static async Task Main(string[] args) {
            var config = Config.Config.LoadConfig();

            var telegramService = new TelegramService(config);
            var botClient = telegramService.CreateBotClient();

            ITelegramSender telegramSender = new TelegramSender(botClient, config.ChatID);

            var rabbitMQService = new RabbitMQService(config);
            rabbitMQService.Connect();

            IMessageConsumer messageConsumer = new MessageConsumer(rabbitMQService, config, telegramSender);

            messageConsumer.StartConsuming();

            var telegramReceiver = new TelegramReceiver(config.TelegramBotToken, telegramSender);
            Task.Run(() => telegramReceiver.StartReceiving());

            var resetEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) => {
                Console.WriteLine("Shutting down...");
                eventArgs.Cancel = true;
                resetEvent.Set();
            };

            resetEvent.WaitOne();

            rabbitMQService.Close();
        }
    }
}
