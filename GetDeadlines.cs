using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using lab4_18.Entity;

namespace lab4_18.Request;

public class GetDeadlines
{
    private const string ServerAddress = "localhost"; // Адреса сервера
    private const int ServerPort = 5000; // Порт сервера

    // Метод для отримання списку студентів через TCP
    public static async Task<List<Deadline>> Send(string speciality)
    {
        var requestModel = new
        {
            command = "getdeadlinesbyspeciality",
            Speciality = speciality
        };

        var request = JsonSerializer.Serialize(requestModel);
        var bytesToSend = Encoding.UTF8.GetBytes(request);

        using (var client = new TcpClient(ServerAddress, ServerPort))
        {
            using (var stream = client.GetStream())
            {
                try
                {
                    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

                    using (var memoryStream = new MemoryStream())
                    {
                        var buffer = new byte[1024];
                        int bytesRead;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await memoryStream.WriteAsync(buffer, 0, bytesRead);
                        }

                        string response = Encoding.UTF8.GetString(memoryStream.ToArray());
                        Console.WriteLine(response);
                        
                        var responseObject = JsonSerializer.Deserialize<ResponseWrapper>(response);
                        if (responseObject.success)
                        {
                            return responseObject.deadlines;
                        }
                        
                        MessageBox.Show(responseObject.message);
                        return new List<Deadline>();
                    }
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                    return new List<Deadline>();
                }
            }
        }
    }

    // Клас для обгортки відповіді сервера
    private class ResponseWrapper
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<Deadline> deadlines { get; set; }
    }
}