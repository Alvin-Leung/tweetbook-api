using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tweetbook.Options;

namespace Tweetbook
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data.DataContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<Data.DataContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger Generator as a service. We can define 1 or more Swagger documents here
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                { 
                    Title = "Tweetbook API",
                    Version = "v1",
                    Description = "Test documentation for the Tweetbook API",
                    TermsOfService = "https://example.com/terms",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Alvin Leung",
                        Email = string.Empty,
                        Url = "https://gooddevbaddev.com",
                    },
                    License = new Swashbuckle.AspNetCore.Swagger.License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license",
                    }
                });
            });
        }

        // Middleware should be enabled here
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            this.ConfigureSwagger(app);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc();
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions); // Note: This line binds our in-memory object to the appsettings json file

            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });
        }
    }
}
