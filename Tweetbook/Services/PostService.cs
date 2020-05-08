using System;
using System.Collections.Generic;
using System.Linq;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IDataService<Post>
    {
        private List<Post> posts;

        public PostService()
        {
            this.posts = new List<Post>();

            for (var i = 0; i < 5; i++)
            {
                this.posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post Name {i}"
                });
            }
        }

        public IEnumerable<Post> GetAll()
        {
            return this.posts;
        }

        public Post Get(Guid Id)
        {
            return this.posts.SingleOrDefault(post => post.Id == Id);
        }

        public void Add(Post post)
        {
            this.posts.Add(post);
        }
    }
}
