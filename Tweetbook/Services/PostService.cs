using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IDataService<Post>
    {
        private readonly DataContext dataContext;

        public PostService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await this.dataContext.Posts.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid Id)
        {
            return await this.dataContext.Posts.SingleOrDefaultAsync(post => post.Id == Id);
        }

        public async Task<bool> CreateAsync(Post post)
        {
            this.dataContext.Posts.Add(post);
            var numCreated = await this.dataContext.SaveChangesAsync();

            return numCreated > 0;
        }

        public async Task<bool> UpdateAsync(Post updatedPost)
        {
            this.dataContext.Posts.Update(updatedPost);
            var numUpdated = await this.dataContext.SaveChangesAsync();

            return numUpdated > 0;
        }

        public async Task<bool> DeleteAsync(Guid postId)
        {
            var postToDelete = await this.GetByIdAsync(postId);

            if (postToDelete == default)
            {
                return false;
            }

            this.dataContext.Posts.Remove(postToDelete);
            var numDeleted = await this.dataContext.SaveChangesAsync();

            return numDeleted > 0;
        }
    }
}
