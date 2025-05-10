using Microsoft.AspNetCore.Identity;
using PSK2025.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using SystemTask = System.Threading.Tasks.Task;


namespace PSK2025.Data.Seeding;

public static class RoleSeeder
{
    public static async SystemTask SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var roleName in Enum.GetNames(typeof(Roles)))
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"Failed to create role {roleName}");
                    continue;
                }

                await roleManager.AddClaimAsync(role, new Claim(ClaimTypes.Role, roleName));
            }
        }
    }
}
