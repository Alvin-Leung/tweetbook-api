using System;
using System.Collections.Generic;

namespace Tweetbook.Controllers.V1.Responses
{
    public class PostResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
