using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface IPostService : IDataService<Post>
    {
        public Task<bool> UserOwnsPostAsync(string userId, Guid postId);

        Task<List<Tag>> GetAllTagsAsync();
    }
}
