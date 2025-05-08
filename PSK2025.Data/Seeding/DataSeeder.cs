using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        const string defaultUserName = "admin";

        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {

                return;
            }
        }

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
            if (!result.Succeeded)
            {

                return;
            }
        }

        var userRoles = await userManager.GetRolesAsync(user);
        if (!userRoles.Contains(roleName))
        {
            var roleResult = await userManager.AddToRoleAsync(user, roleName);
            if (roleResult.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, roleName));
            }
            else
            {

            }
        }
    }
}