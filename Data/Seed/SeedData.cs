using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Data.Seed;

public class SeedData
{
      public static async Task Initialize(IServiceProvider serviceProvider)
      {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Seed Roles
            await SeedRolesAsync(roleManager, context);

            // Seed Admin User
            await SeedAdminUserAsync(userManager, roleManager);
      }

      static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager, DatabaseContext context)
      {
            string[] roleNames = ["Admin", "User"];

            foreach (var roleName in roleNames)
            {
                  var roleExist = await roleManager.RoleExistsAsync(roleName);
                  if (!roleExist)
                  {
                        var role = new IdentityRole<int>
                        {
                              Name = roleName,
                              NormalizedName = roleName.ToLower(),
                        };

                        await roleManager.CreateAsync(role);
                  }
            }
      }
     
      static async Task SeedAdminUserAsync(UserManager<IdentityUser<int>> userManager, RoleManager<IdentityRole<int>> roleManager)
      {
           const string ADMIN_EMAIL = "admin@TodoApi.com";
           const string ADMIN_PASS = "Admin@123";
           const string ADMIN_ROLE = "Admin";
            var adminUser = await userManager.FindByEmailAsync(ADMIN_EMAIL);

            if (adminUser == null)
            {
                  adminUser = new IdentityUser<int>
                  {
                        UserName = ADMIN_EMAIL,
                        Email = ADMIN_EMAIL,
                        EmailConfirmed = true,
                        PhoneNumber = "+1234567890",
                  };

                  var result = await userManager.CreateAsync(adminUser, ADMIN_PASS);

                  if (result.Succeeded)
                  {
                        if (await roleManager.RoleExistsAsync(ADMIN_ROLE))
                              await userManager.AddToRoleAsync(adminUser, ADMIN_ROLE);
                        await userManager.UpdateAsync(adminUser);
                  }
            }
      }
}
