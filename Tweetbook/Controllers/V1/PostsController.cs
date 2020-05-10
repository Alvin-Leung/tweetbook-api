using Microsoft.AspNetCore.Mvc;
using System;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    public class PostsController : Controller
    {
        private readonly IDataService<Post> postService;

        public PostsController(IDataService<Post> postService)
        {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(this.postService.GetAll());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute]Guid postId)
        {
            var foundPost = this.postService.GetById(postId);
            
            if (foundPost == null)
            {
                return NotFound();
            }

            return Ok(foundPost);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute]Guid postId, [FromBody]UpdatePostRequest postRequest)
        {
            var post = new Post
            {
                Id = postId,
                Name = postRequest.Name
            };

            if (!this.postService.Update(post))
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
        public IActionResult Create([FromBody]CreatePostRequest postRequest) // The FromBody attribute gives the framework a clue as to where the posted data is (from the body in this case)
        {
            if (postRequest.Id == Guid.Empty)
            {
                postRequest.Id = Guid.NewGuid();
            }

            // Make sure to map the request to your domain object! This is why we created an additional CreatePostRequest class
            // What we expose to the client is a (versioned) contract/interface. Never mix up your contracts with domain objects,
            // as you may want to change the contract, but keep the domain objects the same in the future.
            var post = new Post
            {
                Id = postRequest.Id,
                Name = postRequest.Name
            };

            this.postService.Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", postRequest.Id.ToString())}";

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
        public IActionResult Delete([FromRoute]Guid postId)
        {
            if (!this.postService.Delete(postId))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
