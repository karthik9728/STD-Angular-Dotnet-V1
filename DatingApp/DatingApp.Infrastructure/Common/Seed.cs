using DatingApp.Domain.Models;
using DatingApp.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DatingApp.Infrastructure.Common
{
    public class Seed
    {
        public static async Task SeedUsers(ApplicationDbContext _dbContext)
        {
            if (await _dbContext.Users.AnyAsync()) return;

            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "UserSeedData.json");

            if (File.Exists(jsonFilePath))
            {
                var userData = await File.ReadAllTextAsync(jsonFilePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();

                    user.UserName = user.UserName.ToLower();

                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password@123"));

                    user.PasswordSalt = hmac.Key;

                    _dbContext.Users.Add(user);
                }

                await _dbContext.SaveChangesAsync();
            }

        }
    }
}
