﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramNofitier.Config;
using TelegramNofitier.RabbitMQ.Interfaces;
using TelegramNofitier.Telegram;
using TelegramNofitier.Telegram.Interfaces;

namespace TelegramNofitier.RabbitMQ {
    public class MessageConsumer : IMessageConsumer {
        private readonly RabbitMQService _rabbitMQService;
        private readonly Config.Config _config;
        private readonly ITelegramSender _telegramSender;

        public MessageConsumer(RabbitMQService rabbitMQService, Config.Config config, ITelegramSender telegramSender) {
            _rabbitMQService = rabbitMQService;
            _config = config;
            _telegramSender = telegramSender;
        }

        public void StartConsuming() {
            IModel channel = _rabbitMQService.GetChannel();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (sender, args) => {

                var body = args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Message: {message}");
                await _telegramSender.SendMessage(message);

                channel.BasicAck(args.DeliveryTag, false);
            };
            string consumerTag = channel.BasicConsume(_config.RabbitMQ_QueueName, false, consumer);
        }
    }
}
