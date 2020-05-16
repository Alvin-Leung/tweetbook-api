using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IDataService<Post> postService;

        public PostsController(IDataService<Post> postService)
        {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await this.postService.GetAllAsync());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            var foundPost = await this.postService.GetByIdAsync(postId);
            
            if (foundPost == null)
            {
                return NotFound();
            }

            return Ok(foundPost);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId, [FromBody]UpdatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = postRequest.Name
            };

            if (!await this.postService.UpdateAsync(post))
            {
                return NotFound();
            }

            var postResponse = new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            };

            return Ok(postResponse);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest postRequest) // The FromBody attribute gives the framework a clue as to where the posted data is (from the body in this case)
        {
            // Make sure to map the request to your domain object! This is why we created an additional CreatePostRequest class
            // What we expose to the client is a (versioned) contract/interface. Never mix up your contracts with domain objects,
            // as you may want to change the contract, but keep the domain objects the same in the future.
            var post = new Post
            {
                Name = postRequest.Name
            };

            await this.postService.CreateAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";

            // As with the request object, also make sure to use a versioned contract class for the response.
            // Both request and response look the same right now, but this could easily change in the future!
            var response = new PostResponse
            {
                Id = post.Id,
                Name = post.Name
            };

            return Created(locationUri, response);
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid postId)
        {
            if (!await this.postService.DeleteAsync(postId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
