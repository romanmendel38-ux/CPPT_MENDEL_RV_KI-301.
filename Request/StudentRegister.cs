using System.Text.Json;
using System.Windows;

namespace lab4_18.Request
{
    public class StudentRegister
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції запиту на реєстрацію
        public async Task<bool> RegisterAsync(string username, string password, string email)
        {
            await Task.Delay(200); // Симуляція затримки мережі

            // Успішна "реєстрація"
            bool registrationSuccess = true;

            // Формуємо фейкову відповідь від сервера
            var responseObject = new ResponseWrapper
            {
                success = registrationSuccess,
                message = registrationSuccess
                    ? $"Користувач \"{username}\" успішно зареєстрований з email: {email}"
                    : "Не вдалося зареєструвати користувача."
            };

            // Виводимо у консоль JSON для налагодження
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            // Завжди повертаємо успіх
            return registrationSuccess;
        }

        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}