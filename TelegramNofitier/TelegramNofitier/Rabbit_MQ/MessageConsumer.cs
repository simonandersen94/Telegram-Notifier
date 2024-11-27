using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramNofitier.Config;
using TelegramNofitier.RabbitMQ.Interfaces;
using TelegramNofitier.Telegram;

namespace TelegramNofitier.RabbitMQ {
    public class MessageConsumer : IMessageConsumer {
        private readonly RabbitMQService _rabbitMQService;
        private readonly Config.Config _config;
        private readonly TelegramService _telegramService;

        public MessageConsumer(RabbitMQService rabbitMQService, Config.Config config, TelegramService telegramService) {
            _rabbitMQService = rabbitMQService;
            _config = config;
            _telegramService = telegramService;
        }

        public void StartConsuming() {
            IModel channel = _rabbitMQService.GetChannel();

            channel.ExchangeDeclare(_config.RabbitMQ_ExchangeName, ExchangeType.Direct);
            channel.QueueDeclare(_config.RabbitMQ_QueueName, false, false, false, null);
            channel.QueueBind(_config.RabbitMQ_QueueName, _config.RabbitMQ_ExchangeName, _config.RabbitMQ_RoutingKey, null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, args) => {

                var body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Message: {message}");
                await _telegramService.SendMessageAsync(message);

                channel.BasicAck(args.DeliveryTag, false);
            };
            string consumerTag = channel.BasicConsume(_config.RabbitMQ_QueueName, false, consumer);
        }
    }
}
