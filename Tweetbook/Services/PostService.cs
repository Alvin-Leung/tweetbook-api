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

        public Post GetById(Guid Id)
        {
            return this.posts.SingleOrDefault(post => post.Id == Id);
        }

        public void Add(Post post)
        {
            this.posts.Add(post);
        }

        public bool Update(Post updatedPost)
        {
            var postToOverwrite = this.GetById(updatedPost.Id);

            if (postToOverwrite == null)
            {
                return false;
            }

            var index = this.posts.IndexOf(postToOverwrite);
            this.posts[index] = updatedPost;
            return true;
        }

        public bool Delete(Guid postId)
        {
            var postToDelete = this.GetById(postId);

            if (postToDelete == default)
            {
                return false;
            }

            this.posts.Remove(postToDelete);
            return true;
        }
    }
}
