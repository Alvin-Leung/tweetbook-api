using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetbook.Installers;
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

        /// <summary>
        /// Configures services for the application
        /// </summary>
        /// <remarks>
        /// One way to install services is to create one or more interfaces <see cref="IInstaller"/> that have child implementations for different domains of the code.
        /// This is a lot cleaner than simply implementing all service installation logic here; it also opens up the opportunity to modularize installations such that
        /// installs can be customized with only the services they need. See <see cref="InstallerExtensions"/> for the implementation.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallServicesInAssembly(this.Configuration);
        }

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

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc();
        }

        /// <summary>
        /// Configures Swagger
        /// </summary>
        /// <remarks>
        /// When dealing with configurable services, it's usually a good idea to define configuration values in appsettings.json or similar, and then bind the values to objects
        /// at runtime. For different environments (dev, production, etc.), we can then easily modify or swap out the configuration file to get different application setup.
        /// </remarks>
        private void ConfigureSwagger(IApplicationBuilder app)
        {
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

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
