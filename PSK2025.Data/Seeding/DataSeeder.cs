using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSK2025.Data.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


        const string roleName = "Admin";
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        const string defaultUserName = "admin";
        var user = await userManager.FindByNameAsync(defaultUserName);
        if (user == null)
        {
            user = new User
            {
                UserName = defaultUserName,
                Email = "admin@example.com",
                EmailConfirmed = true,
                FirstName = "Test",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(user, "Admin@123"); 

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Seeding error: {error.Description}");
                }
            }
        }
    }
}
