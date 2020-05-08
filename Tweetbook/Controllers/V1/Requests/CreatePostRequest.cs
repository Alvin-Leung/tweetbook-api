using System;

namespace Tweetbook.Controllers.V1.Requests
{
    public class CreatePostRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
