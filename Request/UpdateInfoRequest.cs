using System.Globalization;
using System.Text.Json;
using System.Windows;
using lab4_18.Entity;

namespace lab4_18.Request
{
    public class UpdateInfoRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для симуляції оновлення інформації про студента
        public async Task<bool> Update(string username, StudentDetailedInfo detailedInfo)
        {
            await Task.Delay(200); // Симуляція затримки мережі

            try
            {
                // Призначаємо Username для послідовності
                detailedInfo.Username = username;

                // Формуємо псевдо-запит
                var requestModel = new
                {
                    command = "upsert",
                    Username = detailedInfo.Username,
                    Name = detailedInfo.Name,
                    University = detailedInfo.University,
                    Specialization = detailedInfo.Specialization,
                    Email = detailedInfo.Email,
                    Rating = detailedInfo.Rating.ToString(CultureInfo.InvariantCulture)
                };

                // Симульована успішна відповідь
                var responseObject = new ResponseWrapper
                {
                    success = true,
                    message = $"Профіль користувача \"{detailedInfo.Name}\" успішно оновлено."
                };

                // Вивід псевдо-запиту і відповіді в консоль
                string request = JsonSerializer.Serialize(requestModel, new JsonSerializerOptions { WriteIndented = true });
                string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });

                Console.WriteLine("[SIMULATED REQUEST]");
                Console.WriteLine(request);
                Console.WriteLine("[SIMULATED RESPONSE]");
                Console.WriteLine(response);

                // Завжди повертаємо успіх
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при оновленні даних: {ex.Message}");
                return false;
            }
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
        }
    }
}
