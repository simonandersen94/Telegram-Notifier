namespace TelegramNofitier {
    internal class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello, World!");
            try {
                var config = Config.Config.LoadConfig();
                Console.WriteLine($"API Key: {config.RabbitMQ_QueueName}");
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
