using System;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface IPostService : IDataService<Post>
    {
        public Task<bool> UserOwnsPost(string userId, Guid postId);
    }
}
