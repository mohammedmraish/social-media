using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using social_media.Data;
using social_media.Entity;
using soicalMedia.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace soicalMedia.Data
{
    public class Seed
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public Seed(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public  async Task SeedUsers()
        {

            if (await userManager.Users.AnyAsync())
                return;

           
            var userData =  System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole{ Name="Member"},
                new AppRole{ Name="Admin"},
                new AppRole{ Name="Moderator"},
            };

            foreach (var role in roles)
            { 
              await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName=user.UserName.ToLower();

                await userManager.CreateAsync(user,"Password");

                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin,"Password");
            
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });

        }

    }
}
