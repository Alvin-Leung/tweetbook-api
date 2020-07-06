namespace Tweetbook.Contract.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        /// <summary>
        /// Constants for <see cref="Tweetbook.Domain.Post"/> related routes
        /// </summary>
        /// <remarks>
        /// Conventions:
        /// - Use nouns for endpoint names
        /// - The <see cref="Create"/> endpoint is usually named the same as the <see cref="GetAll"/> endpoint
        /// </remarks>
        public static class Posts
        {
            public const string GetAll = Base + "/posts";

            public const string Get = Base + "/posts/{postId}";

            public const string Update = Base + "/posts/{postId}";

            public const string Create = Base + "/posts";

            public const string Delete = Base + "/posts/{postId}";
        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags";
        }

        /// <summary>
        /// Constants for Identity-related routes
        /// </summary>
        /// <remarks>
        /// Note the routes below use verbs instead of nouns, which is non-standard for RESTful APIs. We would 
        /// typically define identity related routes on a separate identity server, with the API hosted on this server
        /// not necessarily needing to be RESTful. However, to demo both RESTful API and identity functionality in 
        /// one project, the identity routes have been defined below.
        /// </remarks>
        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
