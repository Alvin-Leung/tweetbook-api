using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Data.DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() // 2. Add role related services
                .AddEntityFrameworkStores<Data.DataContext>();

            services.AddScoped<PostService>();
            services.AddScoped<IDataService<Post, Guid>>(serviceProvider => serviceProvider.GetService<PostService>());
            services.AddScoped<IPostService>(serviceProvider => serviceProvider.GetService<PostService>());

            services.AddScoped<IDataService<Tag, string>, TagService>(); // 3. Add a scoped tag service
        }
    }
}
