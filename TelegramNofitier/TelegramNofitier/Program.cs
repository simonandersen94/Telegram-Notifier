using TelegramNofitier.RabbitMQ;
using TelegramNofitier.Config;

namespace TelegramNofitier {
    internal class Program {
        static void Main(string[] args) {
            var config = Config.Config.LoadConfig();

            var rabbitMQService = new RabbitMQService(config);
            rabbitMQService.Connect();

            var messageConsumer = new MessageConsumer(rabbitMQService, config);
            messageConsumer.StartConsuming();

            Console.ReadLine();

            rabbitMQService.Close();
        }
    }
}
