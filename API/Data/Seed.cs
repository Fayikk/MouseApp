using System.Text.Json;
using API.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {

        public static async Task SeedUsers(UserManager<BusinessUser> userManager,RoleManager<BusinessRole> roleManager){
            if(await userManager.Users.AnyAsync()) return;
            var userData = await System.IO.File.ReadAllTextAsync("C:/Users/fayik/MouseApp/API/Data/UserSeedData.json");
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BusinessUser>>(userData);
            var roles = new List<BusinessRole>
            {
                new BusinessRole{Name = "Member"},
                new BusinessRole{Name = "Admin"},
                new BusinessRole{Name = "Moderator"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                await userManager.CreateAsync(user,"Pa$$w0rd");
                await userManager.AddToRoleAsync(user,"Member");
            
            }

            var admin = new BusinessUser
            {
                UserName="admin"
            };

            await userManager.CreateAsync(admin , "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin , new[] {"Admin" , "Moderator"});
        }        
    }
}