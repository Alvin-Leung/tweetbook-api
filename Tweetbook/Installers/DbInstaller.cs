﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .AddEntityFrameworkStores<Data.DataContext>();

            services.AddScoped<PostService>();
            services.AddScoped<IDataService<Post>>(x => x.GetService<PostService>());
            services.AddScoped<IPostService>(x => x.GetService<PostService>());
        }
    }
}
