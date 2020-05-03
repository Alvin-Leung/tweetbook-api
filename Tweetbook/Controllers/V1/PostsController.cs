using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Tweetbook.Contract.V1;
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
    }
}
