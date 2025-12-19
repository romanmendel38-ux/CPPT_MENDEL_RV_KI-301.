using System.Text.Json;
using System.Windows;
using lab4_18.Entity;

namespace lab4_18.Request
{
    public class GetStudents
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        // Метод для отримання списку студентів (симуляція)
        public async Task<List<Student>> FetchStudentsAsync()
        {
            await Task.Delay(200); // Симуляція мережевої затримки

            // Вигадані тестові студенти
            var students = new List<Student>
            {
                new Student { Name = "Олександр Петренко", Username = "opetrenko", Rating = 95.2 },
                new Student { Name = "Марія Коваль", Username = "mkoval", Rating = 88.7 },
                new Student { Name = "Ігор Сидоренко", Username = "isydorenko", Rating = 91.4 },
                new Student { Name = "Наталія Ткачук", Username = "ntkachuk", Rating = 85.6 },
                new Student { Name = "Дмитро Павленко", Username = "dpavlenko", Rating = 90.1 }
            };

            // Симульована успішна відповідь
            var responseObject = new ResponseWrapper
            {
                success = true,
                message = "Success",
                profiles = students
            };

            // Вивід у консоль для наочності
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            return students;
        }

        // Клас для обгортки відповіді сервера
        private class ResponseWrapper
        {
            public bool success { get; set; }
            public string message { get; set; }
            public List<Student> profiles { get; set; }
        }
    }
}