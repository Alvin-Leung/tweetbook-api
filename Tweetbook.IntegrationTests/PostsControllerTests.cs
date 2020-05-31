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
using Tweetbook.Controllers.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Data;

namespace Tweetbook.IntegrationTests
{
    /// <summary>
    /// Integration tests for the <see cref="PostsController"/>
    /// </summary>
    /// <remarks>
    /// The tests in this class make usage of the <see cref="WebApplicationFactory{TEntryPoint}"/> class, which has been around since ASP.NET Core 2.1. One useful thing about
    /// this class is it allows you to simulate the environment that your ASP.NET Core app usually runs in, and allows for substitution of resource heavy services, such as
    /// database services. As such, the example tests below have been set up to communicate with an in-memory database instead of an actual database. While this solution is
    /// not appropriate for all situations due to behavioural/performance differences when compared to real databases, it can be used to speed up the execution of tests
    /// and to quickly implement integration tests without the need for database setup/teardown logic.
    /// </remarks>
    [TestFixture]
    public class PostsControllerTests
    {
        private HttpClient client;

        /// <summary>
        /// Runs required setup before every test
        /// </summary>
        /// <remarks>
        /// As mentioned in the class comments, we use <see cref="WebApplicationFactory{TEntryPoint}"/> to substitute usage of a real database with an in-memory one.
        /// Since NUnit is used as a testing framework here, the <see cref="HttpClient"/> has been created anew for for each test method as an easy solution to isolate
        /// database changes from each other. To achieve the same simplicity, consider using a testing framework such as XUnit which creates a new instance of the test
        /// class for each method by default.
        /// </remarks>
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

            this.client = appFactory.CreateClient();
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
