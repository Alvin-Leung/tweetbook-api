﻿using System.Collections.Generic;

namespace Tweetbook.Controllers.V1.Responses
{
    public class AuthFailResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
