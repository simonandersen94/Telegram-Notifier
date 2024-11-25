using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TelegramNofitier.Config {
    public class Config {
        public string? RabbitMQ_Uri { get; set; }
        public string? RabbitMQ_ClientProvidedName { get; set; }
        public string? RabbitMQ_ExchangeName { get; set; }
        public string? RabbitMQ_RoutingKey { get; set; }
        public string? RabbitMQ_QueueName { get; set; }

        private static readonly string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Config/Config.json");

        public static Config LoadConfig() {
            if (!File.Exists(ConfigPath)) {
                throw new FileNotFoundException($"Config file not found at: {ConfigPath}");
            }
            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<Config>(json);
        }
    }
}
