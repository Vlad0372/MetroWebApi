using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace MetroWebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            #region UserRolesInitialize
            using (var serviceScope = host.Services.CreateScope())
            {
                string adminEmail = "admin@gmail.com";
                string adminPassword = "adminpass123";

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if(!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    var userRole = new IdentityRole("User");
                    await roleManager.CreateAsync(userRole);
                }
                if (await userManager.FindByNameAsync(adminEmail) == null)
                {
                    IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = adminEmail };
                    IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                        await userManager.AddToRoleAsync(admin, "User");
                    }
                }
            }
            #endregion

            await host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
