using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Text;
using Tweetbook.Options;
using Tweetbook.Services;

namespace Tweetbook.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            this.InstallAuthentication(services, configuration);
            this.InstallSwagger(services);
        }

        private void InstallAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

            var tokenValidationParameters = this.GetTokenValidationParameters(jwtSettings);

            services.AddSingleton(jwtSettings);
            services.AddSingleton(tokenValidationParameters);
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
            });
        }

        private TokenValidationParameters GetTokenValidationParameters(JwtSettings jwtSettings)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        }

        private void InstallSwagger(IServiceCollection services)
        {
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

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[0]}
                };

                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                x.AddSecurityRequirement(security);
            });
        }
    }
}
