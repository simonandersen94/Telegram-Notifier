using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramNofitier.Telegram.Interfaces;
using TelegramNofitier.TempData;

namespace TelegramNofitier.Telegram {
    public class TelegramReceiver {
        private readonly ITelegramBotClient _botClient;
        private readonly ITelegramSender _telegramSender;
        private static readonly HttpClient _httpClient = new();

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

            string text = message.Text?.Trim();

            Console.WriteLine($"Received message: {text}");

            if (text == "__temp") {
                string tempMessage = await GetTemperatureMessage();
                await _telegramSender.SendMessage(tempMessage);
            }
        }

        private async Task<string> GetTemperatureMessage() {
            try {
                string apiUrl = "http://localhost:1900/api/temperatures/newest";
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode) {
                    Console.WriteLine($"API request failed: {response.StatusCode}");
                    return "Kunne ikke hente temperaturdata.";
                }

                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"RAW JSON: {json}");

                var tempData = JsonSerializer.Deserialize<TemperatureData>(json, new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true
                });

                if (tempData == null) {
                    Console.WriteLine("Deserialization returned null.");
                    return "Kunne ikke læse temperaturdata.";
                }

                return $"Temperatur i stuen: {tempData.Temp}°C ({tempData.Date})";
            } catch (Exception ex) {
                Console.WriteLine($"Fejl ved hentning af temperatur: {ex.Message}");
                return "Fejl ved hentning af temperatur.";
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) {
            Console.WriteLine($"Telegram Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
