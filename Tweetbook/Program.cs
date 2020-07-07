using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Services;

namespace Tweetbook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope()) 
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                await dbContext.Database.MigrateAsync();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(Policies.Roles.Admin))
                {
                    var adminRole = new IdentityRole(Policies.Roles.Admin);
                    await roleManager.CreateAsync(adminRole);
                }

                if (!await roleManager.RoleExistsAsync(Policies.Roles.Poster))
                {
                    var posterRole = new IdentityRole(Policies.Roles.Poster);
                    await roleManager.CreateAsync(posterRole);
                }
            }

            await host.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
