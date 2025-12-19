using System.Text.Json;
using System.Windows;

namespace lab4_18.Request
{
    public class StudentLogin
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції запиту на логін
        public async Task<bool> LoginAsync(string username, string password)
        {
            await Task.Delay(200); // Симуляція мережевої затримки

            // Симульована логіка "перевірки" (всі користувачі успішно логіняться)
            bool loginSuccess = true;

            // Створюємо псевдо-відповідь від сервера
            var responseObject = new ResponseWrapper
            {
                success = loginSuccess,
                message = loginSuccess
                    ? $"Користувач \"{username}\" успішно увійшов у систему."
                    : "Невірне ім’я користувача або пароль."
            };

            // Вивід JSON-відповіді у консоль для наочності
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            // Завжди повертаємо успіх (можна змінити при потребі)
            return loginSuccess;
        }

        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}