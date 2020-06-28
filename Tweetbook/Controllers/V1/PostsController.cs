using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await this.postService.GetAllAsync();

            return Ok(posts.Select(post => new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                Tags = post.Tags.Select(postTag => postTag.TagName)
            }));
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid postId)
        {
            var foundPost = await this.postService.GetByIdAsync(postId);
            
            if (foundPost == null)
            {
                return NotFound();
            }

            return Ok(new PostResponse
            {
                Id = foundPost.Id,
                Name = foundPost.Name,
                Tags = foundPost.Tags.Select(postTag => postTag.TagName)
            });
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid postId, [FromBody]UpdatePostRequest postRequest)
        {
            var userOwnsPost = await this.postService.UserOwnsPostAsync(HttpContext.GetUserId(), postId);

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var post = await this.postService.GetByIdAsync(postId);
            post.Name = postRequest.Name;
            post.Tags = postRequest.Tags.Select(tagName => new PostTag { TagName = tagName, PostId = post.Id }).ToList();

            if (!await this.postService.UpdateAsync(post))
            {
                return NotFound();
            }

            return Ok(new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                Tags = post.Tags.Select(postTag => postTag.TagName)
            });
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody]CreatePostRequest postRequest) // The FromBody attribute gives the framework a clue as to where the posted data is (from the body in this case)
        {
            // Make sure to map the request to your domain object! This is why we created an additional CreatePostRequest class
            // What we expose to the client is a (versioned) contract/interface. Never mix up your contracts with domain objects,
            // as you may want to change the contract, but keep the domain objects the same in the future.
            var newPostId = Guid.NewGuid();
            var post = new Post
            {
                Id = newPostId,
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
                Tags = postRequest.Tags.Select(tagName => new PostTag { TagName = tagName, PostId = newPostId }).ToList()
            };

            await this.postService.CreateAsync(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString())}";

            // As with the request object, also make sure to use a versioned contract class for the response.
            // Both request and response look the same right now, but this could easily change in the future!
            var response = new PostResponse
            {
                Id = post.Id,
                Name = post.Name,
                Tags = post.Tags.Select(postTag => postTag.TagName)
            };

            return Created(locationUri, response);
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute]Guid postId)
        {
            var userOwnsPost = await this.postService.UserOwnsPostAsync(HttpContext.GetUserId(), postId);

            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            if (!await this.postService.DeleteAsync(postId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
