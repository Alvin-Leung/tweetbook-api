using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Data;

namespace Tweetbook.IntegrationTests
{
    public sealed class TestScope : IDisposable
    {
        private readonly HttpClient client;

        public TestScope()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "testDatabase"));
                    });
                });

            client = appFactory.CreateClient();
        }

        public async Task AuthenticateAsync()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await this.client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest 
            { 
                Email = "test@integration.com", Password = "myP@ssword1!" 
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}