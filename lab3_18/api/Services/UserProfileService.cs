using System.Globalization;
using System.Text.RegularExpressions;
using lab3_18.api.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3_18.api.Services;

public class UserProfileService
{
    private readonly AppDbContext _context;

    public UserProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile> UpsertUserProfile(UserProfile model)
    {
        // Перевірка, чи юзернейм існує в таблиці користувачів
        var userExists = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == model.Username);
        if (userExists == null)
        {
            throw new InvalidOperationException("Username does not exist.");
        }

        // Перевірка валідності даних
        if (string.IsNullOrWhiteSpace(model.Name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (model.Name.Length < 5 || model.Name.Length > 50)
        {
            throw new ArgumentException("Name must be between 5 and 50 characters.");
        }


        if (string.IsNullOrWhiteSpace(model.University))
        {
            throw new ArgumentException("University cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(model.Specialization))
        {
            throw new ArgumentException("Specialization cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(model.Email))
        {
            throw new ArgumentException("Email cannot be empty.");
        }

        // Перевірка діапазону оцінки
        if (model.Rating < 1 || model.Rating > 100)
        {
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 100.");
        }

        // Перевірка формату електронної пошти
        if (!Regex.IsMatch(model.Email, @"^[^@\s]+@lpnu\.com$"))
        {
            throw new ArgumentException("Email must be in a valid format and end with lpnu.com.");
        }

        // Перевірка, чи профіль з таким юзернеймом уже існує
        var existingProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Username == model.Username);

        if (existingProfile != null)
        {
            // Оновлення даних профілю
            existingProfile.Name = model.Name;
            existingProfile.University = model.University;
            existingProfile.Specialization = model.Specialization;
            existingProfile.Email = model.Email;
            existingProfile.Rating = model.Rating;
            _context.UserProfiles.Update(existingProfile);
        }
        else
        {
            // Додавання нового профілю
            _context.UserProfiles.Add(model);
        }

        await _context.SaveChangesAsync();
        return model;
    }


    public async Task<IEnumerable<UserProfile>> GetUserProfiles()
    {
        return await _context.UserProfiles.ToListAsync();
    }

    public async Task<UserProfile> GetUserProfileByName(string userName)
    {
        var userProfile = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Username == userName);

        if (userProfile == null)
        {
            return new UserProfile { Username = userName };
        }

        return userProfile;
    }

    public async Task<IEnumerable<UserProfile>> GetUserProfilesByName(string name)
    {
        return await _context.UserProfiles
            .Where(u => u.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<IEnumerable<UserProfile>> GetUserProfilesBySpecialization(string specialization)
    {
        return await _context.UserProfiles
            .Where(u => u.Specialization.Contains(specialization))
            .ToListAsync();
    }

    public async Task<bool> AddDeadline(Deadline deadline)
    {
        if (string.IsNullOrWhiteSpace(deadline.Subject))
        {
            throw new ArgumentException("Subject cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(deadline.Speciality))
        {
            throw new ArgumentException("Speciality cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(deadline.Date))
        {
            throw new ArgumentException("Date cannot be empty.");
        }

        if (!DateTime.TryParse(deadline.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            throw new ArgumentException("Invalid date format.");
        }

        try
        {
            _context.Deadlines.Add(deadline);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to add deadline: {ex.Message}");
        }
    }

    public async Task<List<DeadlineWithStatus>> GetDeadlinesBySpeciality(string speciality)
    {
        if (string.IsNullOrWhiteSpace(speciality))
        {
            throw new ArgumentException("Speciality cannot be empty.");
        }

        try
        {
            // Перевіряємо чи існує спеціальність
            bool specialityExists = await _context.Deadlines
                .AnyAsync(d => d.Speciality == speciality);

            if (!specialityExists)
            {
                throw new InvalidOperationException($"Speciality '{speciality}' not found.");
            }

            var deadlines = await _context.Deadlines
                .Where(d => d.Speciality == speciality)
                .ToListAsync();

            DateTime currentDate = DateTime.Now;

            var result = deadlines.Select(d => new DeadlineWithStatus
            {
                Id = d.Id,
                Subject = d.Subject,
                Speciality = d.Speciality,
                Date = d.Date,
                Status = DateTime.TryParse(d.Date, out DateTime deadlineDate) && deadlineDate < currentDate
                    ? "Пропущено"
                    : "Не пропущено"
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to retrieve deadlines: {ex.Message}");
        }
    }

    public class DeadlineWithStatus
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Speciality { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }
}