using System.Collections.Generic;

namespace Tweetbook.Controllers.V1.Requests
{
    public class UpdatePostRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
