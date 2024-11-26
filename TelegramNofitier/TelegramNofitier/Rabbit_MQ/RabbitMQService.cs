using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramNofitier.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TelegramNofitier.RabbitMQ {
    public class RabbitMQService {
        private readonly Config.Config _config;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQService(Config.Config config) {
        _config = config;
        }

        public void Connect() {
            var factory = new ConnectionFactory {
                Uri = new Uri(_config.RabbitMQ_Uri),
                ClientProvidedName = _config.RabbitMQ_ClientProvidedName
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public IModel GetChannel() {
            return _channel;
        }

        public void Close() {
            _channel.Close();
            _connection.Close();
        }
    }
}
