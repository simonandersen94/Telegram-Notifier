using System;
using System.Threading;
using System.Threading.Tasks;
using TelegramNofitier.Config;
using TelegramNofitier.RabbitMQ;
using TelegramNofitier.RabbitMQ.Interfaces;
using TelegramNofitier.Telegram;
using TelegramNofitier.Telegram.Interfaces;

namespace TelegramNofitier {
    internal class Program {
        static async Task Main(string[] args) {
            var config = Config.Config.LoadConfig();

            var telegramService = new TelegramService(config);
            var botClient = telegramService.CreateBotClient();
            ITelegramSender telegramSender = new TelegramSender(botClient, config.ChatID);

            var cancellationTokenSource = new CancellationTokenSource();

            var rabbitMQTask = Task.Run(async () => {
                RabbitMQService? rabbitMQService = null;
                IMessageConsumer? messageConsumer = null;

                while (!cancellationTokenSource.Token.IsCancellationRequested) {
                    try {
                        rabbitMQService = new RabbitMQService(config);
                        rabbitMQService.Connect();

                        messageConsumer = new MessageConsumer(rabbitMQService, config, telegramSender);
                        messageConsumer.StartConsuming();

                        Console.WriteLine("Now connected to RabbitMQ.");
                        break;
                    } catch (Exception e) {
                        Console.WriteLine($"Error connecting to RabbitMQ: {e.Message}");
                        await Task.Delay(5000, cancellationTokenSource.Token);
                    }
                }

                cancellationTokenSource.Token.WaitHandle.WaitOne();

                rabbitMQService?.Close();
            });

            var telegramReceiver = new TelegramReceiver(config.TelegramBotToken, telegramSender);
            Task.Run(() => telegramReceiver.StartReceiving(), cancellationTokenSource.Token);

            var resetEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) => {
                Console.WriteLine("Shutting down...");
                eventArgs.Cancel = true;
                cancellationTokenSource.Cancel();
                resetEvent.Set();
            };

            resetEvent.WaitOne();
            await rabbitMQTask;
        }
    }
}
