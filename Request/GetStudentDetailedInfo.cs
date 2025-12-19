using System.Text.Json;
using System.Windows;
using lab4_18.Entity;

namespace lab4_18.Request
{
    public class GetStudentDetailedInfo
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для отримання детальної інформації про студента (симуляція)
        public async Task<StudentDetailedInfo> FetchStudentDetailedInfo(string userName)
        {
            await Task.Delay(200); // Симуляція мережевої затримки

            // Статичні тестові дані (мок)
            var studentInfo = new StudentDetailedInfo
            {
                Username = userName,
                Name = "Василь Козак",
                Rating = 92.5,
                University = "Львівський національний університет",
                Specialization = "Комп’ютерна інженерія",
                Email = "vasya@gmail.com"
            };

            // Обгортка успішної відповіді
            var responseObject = new ResponseWrapper
            {
                success = true,
                message = "Success",
                profile = studentInfo
            };

            // Вивід у консоль для наочності
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            // Повернення змодельованих даних
            return studentInfo;
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public StudentDetailedInfo profile { get; set; }
        }
    }
}