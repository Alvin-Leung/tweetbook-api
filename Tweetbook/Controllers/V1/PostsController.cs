using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Domain;

namespace Tweetbook.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> posts;

        public PostsController()
        {
            this.posts = new List<Post>();

            for (var i = 0; i< 5; i++)
            {
                this.posts.Add(new Post { Id = Guid.NewGuid().ToString() });
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(this.posts);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest) // The FromBody attribute gives the framework a clue as to where the posted data is (from the body in this case)
        {
            if (string.IsNullOrEmpty(postRequest.Id))
            {
                postRequest.Id = Guid.NewGuid().ToString();
            }

            // Make sure to map the request to your domain object! This is why we created an additional CreatePostRequest class
            // What we expose to the client is a (versioned) contract/interface. Never mix up your contracts with domain objects,
            // as you may want to change the contract, but keep the domain objects the same in the future.
            var post = new Post { Id = postRequest.Id };

            this.posts.Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = $"{baseUrl}/{ApiRoutes.Posts.Get.Replace("{postId}", postRequest.Id)}";

            // As with the request object, also make sure to use a versioned contract class for the response.
            // Both request and response look the same right now, but this could easily change in the future!
            var response = new PostResponse { Id = post.Id };
            return Created(locationUri, response);
        }
    }
}
