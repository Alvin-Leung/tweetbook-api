using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext dataContext;

        public PostService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await this.dataContext.Posts.Include(post => post.Tags).ToListAsync();
        }

        public async Task<Post> GetAsync(Guid Id)
        {
            return await this.dataContext.Posts
                .Include(post => post.Tags)
                .SingleOrDefaultAsync(post => post.Id == Id);
        }

        public async Task<bool> CreateAsync(Post post)
        {
            post.Tags?.ForEach(postTag => postTag.TagName = postTag.TagName.ToLower());

            await this.AddNewTagsAsync(post);
            this.dataContext.Posts.Add(post);
            var numCreated = await this.dataContext.SaveChangesAsync();

            return numCreated > 0;
        }

        public async Task<bool> UpdateAsync(Post updatedPost)
        {
            updatedPost.Tags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await this.AddNewTagsAsync(updatedPost);
            this.dataContext.Posts.Update(updatedPost);
            var numUpdated = await this.dataContext.SaveChangesAsync();

            return numUpdated > 0;
        }

        public async Task<bool> DeleteAsync(Guid postId)
        {
            var postToDelete = await this.GetAsync(postId);

            if (postToDelete == null)
            {
                return false;
            }

            this.dataContext.Posts.Remove(postToDelete);
            var numDeleted = await this.dataContext.SaveChangesAsync();

            return numDeleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(string userId, Guid postId)
        {
            var foundPost = await this.dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(post => post.Id == postId);

            if (foundPost == null)
            {
                return false;
            }

            return foundPost.UserId == userId;
        }

        private async Task AddNewTagsAsync(Post post)
        {
            foreach (var newTag in post.Tags)
            {
                var matchingTag = await this.dataContext.Tags.SingleOrDefaultAsync(existingTag => existingTag.Name == newTag.TagName);

                if (matchingTag != null)
                {
                    continue;
                }

                this.dataContext.Tags.Add(new Tag 
                { 
                    Name = newTag.TagName,
                    CreatorId = post.UserId,
                    CreatedOn = DateTime.UtcNow
                });
            }
        }
    }
}
