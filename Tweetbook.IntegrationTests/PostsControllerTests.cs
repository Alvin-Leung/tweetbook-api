using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Data;

namespace Tweetbook.IntegrationTests
{
    [TestFixture]
    public class PostsControllerTests
    {
        private HttpClient client;

        [SetUp]
        public void Setup()
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

        [TearDown]
        public void TearDown()
        {
            this.client.Dispose();
        }

        [Test]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            await this.AuthenticateAsync();

            var response = await this.client.GetAsync(ApiRoutes.Posts.GetAll);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(await response.Content.ReadAsAsync<List<PostResponse>>(), Is.Empty);
        }

        [Test]
        public async Task CreatePost_OnEmptyDatabase_IsSuccessful()
        {
            await this.AuthenticateAsync();

            const string expectedName = nameof(CreatePost_OnEmptyDatabase_IsSuccessful);
            var createdPost = await this.CreatePostAsync(new CreatePostRequest { Name = $"{expectedName}" });

            Assert.That(createdPost.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(createdPost.Name, Is.EqualTo(expectedName));
        }

        [Test]
        public async Task GetSingle_WithPost_ReturnsPost()
        {
            await this.AuthenticateAsync();

            var createdPost = await this.CreatePostAsync(new CreatePostRequest { Name = $"{nameof(GetSingle_WithPost_ReturnsPost)}" });

            var response = await this.client.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", createdPost.Id.ToString()));
            var postResponse = await response.Content.ReadAsAsync<PostResponse>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postResponse.Id, Is.EqualTo(createdPost.Id));
            Assert.That(postResponse.Name, Is.EqualTo(createdPost.Name));
        }

        private async Task AuthenticateAsync()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<PostResponse> CreatePostAsync(CreatePostRequest createPostRequest)
        {
            var response = await this.client.PostAsJsonAsync(ApiRoutes.Posts.Create, createPostRequest);
            return await response.Content.ReadAsAsync<PostResponse>();
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await this.client.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = "test@integration.com",
                Password = "myP@ssword1!"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }
    }
}
