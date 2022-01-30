using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class Seed
    {
        public static async Task seedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }

            string userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            List<AppUser> Users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach (var user in Users)
            {
                user.UserName = user.UserName.ToLower();
                using HMACSHA512 hmac = new HMACSHA512();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password1"));
                user.PasswordSalt = hmac.Key;
                context.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}