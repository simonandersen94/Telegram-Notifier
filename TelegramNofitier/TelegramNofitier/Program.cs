using TelegramNofitier.RabbitMQ;
using TelegramNofitier.Config;
using System;
using System.Threading.Tasks;
using TelegramNofitier.Telegram;

namespace TelegramNofitier {
    internal class Program {
        static async Task Main(string[] args) {
            var config = Config.Config.LoadConfig();

            var telegramService = new TelegramService(config);

            var rabbitMQService = new RabbitMQService(config);
            rabbitMQService.Connect();

            var messageConsumer = new MessageConsumer(rabbitMQService, config, telegramService);

            messageConsumer.StartConsuming();

            Console.ReadLine();

            rabbitMQService.Close();
        }
    }
}
