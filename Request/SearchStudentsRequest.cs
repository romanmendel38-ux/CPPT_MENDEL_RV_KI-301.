using System.Text.Json;
using lab4_18.Entity;

namespace lab4_18.Request
{
    public class SearchStudentsRequest
    {
        private const string ServerAddress = "localhost"; // Адреса сервера
        private const int ServerPort = 5000; // Порт сервера

        public async Task<List<Student>> SearchByName(string name)
        {
            await Task.Delay(200); // Симуляція затримки
            return await SendSearchRequest(new { command = "getprofilesbyname", name = name });
        }

        public async Task<List<Student>> SearchBySpecialty(string specialty)
        {
            await Task.Delay(200); // Симуляція затримки
            return await SendSearchRequest(new { command = "getprofilesbyspecialization", specialization = specialty });
        }

        private async Task<List<Student>> SendSearchRequest(object requestModel)
        {
            await Task.Delay(100); // Ще невелика затримка, як у справжньому запиті

            // Вигадані студенти
            var allStudents = new List<Student>
            {
                new Student { Name = "Олег Шевченко", Username = "oshevchenko", Rating = 91.3 },
                new Student { Name = "Катерина Лисенко", Username = "klysenko", Rating = 87.5 },
                new Student { Name = "Максим Гриценко", Username = "mgrytsenko", Rating = 93.8 },
                new Student { Name = "Ірина Черненко", Username = "ichernenko", Rating = 89.4 },
                new Student { Name = "Тарас Бондар", Username = "tbondar", Rating = 85.9 }
            };

            // Симульована фільтрація (як ніби пошук реально працює)
            List<Student> filteredStudents = new();

            var json = JsonSerializer.Serialize(requestModel);
            if (json.Contains("getprofilesbyname"))
            {
                var name = JsonSerializer.Deserialize<Dictionary<string, string>>(json)["name"];
                filteredStudents = allStudents
                    .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else if (json.Contains("getprofilesbyspecialization"))
            {
                // Просто для прикладу — випадкове підмноження
                filteredStudents = allStudents.Take(3).ToList();
            }

            // Якщо нічого не знайдено — повертаємо весь список (щоб результат не був порожній)
            if (filteredStudents.Count == 0)
                filteredStudents = allStudents;

            var responseObject = new ResponseWrapper
            {
                success = true,
                message = "Success",
                profiles = filteredStudents
            };

            // Вивід псевдо-відповіді у консоль
            string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

            return filteredStudents;
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