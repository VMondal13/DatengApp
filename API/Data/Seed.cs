using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class Seed
{   
    public static async Task SeedUsers(DataContext dataContext)
    {
        if(await dataContext.Users.AnyAsync()) 
            return;
        
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        foreach(var user in users)
        {
            using var hmac = new HMACSHA512();
            //AppUser user = new AppUser();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pas$w0rd"));
            user.PasswordSalt = hmac.Key; //randomly generated key

            dataContext.Users.Add(user);
        }

        await dataContext.SaveChangesAsync();
    }

}
