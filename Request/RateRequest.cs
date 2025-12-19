using System.Text.Json;
using System.Windows;

namespace lab4_18.Request
{
    public class RateRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції запиту на оцінювання
        public async Task<bool> RateAsync(string username, string speciality, string rating)
        {
            await Task.Delay(200); // Симуляція мережевої затримки

            // Симульована відповідь від сервера
            var responseObject = new ResponseWrapper
            {
                success = true,
                message = $"Оцінка {rating} для спеціальності \"{speciality}\" від користувача \"{username}\" успішно збережена."
            };

            // Вивід у консоль для наочності
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            // Завжди повертаємо успіх
            return true;
        }

        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}