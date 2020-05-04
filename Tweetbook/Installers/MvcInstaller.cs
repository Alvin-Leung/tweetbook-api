using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tweetbook.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
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
    }
}
