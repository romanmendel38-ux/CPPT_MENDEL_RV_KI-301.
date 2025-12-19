using System.Text.Json;
using System.Windows;
using lab4_18.Entity;

namespace lab4_18.Request;

public class GetDeadlines
{
    private const string ServerAddress = "localhost";
    private const int ServerPort = 5000;

    public static async Task<List<Deadline>> Send(string speciality)
    {
        await Task.Delay(200); // Симуляція затримки мережі

        // Статичні тестові дані
        var deadlines = new List<Deadline>
        {
            new Deadline
            {
                Id = 1,
                Speciality = speciality,
                Subject = "Програмування",
                Date = "2025-10-25",
                Status = "Очікується"
            },
            new Deadline
            {
                Id = 2,
                Speciality = speciality,
                Subject = "Бази даних",
                Date = "2025-11-01",
                Status = "Здано"
            },
            new Deadline
            {
                Id = 3,
                Speciality = speciality,
                Subject = "Комп'ютерні мережі",
                Date = "2025-11-10",
                Status = "Очікується"
            }
        };

        // Симулюємо "успішну" відповідь сервера
        var responseObject = new ResponseWrapper
        {
            success = true,
            message = "Success",
            deadlines = deadlines
        };

        // Лог для наочності
        string response = JsonSerializer.Serialize(responseObject, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine($"[SIMULATED RESPONSE]\n{response}");

        return responseObject.deadlines;
    }

    private class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Deadline> deadlines { get; set; }
    }
}