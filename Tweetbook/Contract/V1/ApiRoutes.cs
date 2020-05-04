namespace Tweetbook.Contract.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAll = Base + "/posts";

            public const string Get = Base + "/posts/{postId}";

            public const string Create = Base + "/posts"; // note the standard for creation is to name the Create endpoint the same as the GetAll endpoint
        }
    }
}
