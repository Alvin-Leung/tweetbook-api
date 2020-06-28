namespace Tweetbook.Contract.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Posts
        {
            public const string GetAll = Base + "/posts"; // note: use nouns for endpoint names

            public const string Get = Base + "/posts/{postId}";

            public const string Update = Base + "/posts/{postId}";

            public const string Create = Base + "/posts"; // note: the standard for creation is to name the Create endpoint the same as the GetAll endpoint

            public const string Delete = Base + "/posts/{postId}";
        }

        public static class Tags
        {
            public const string GetAll = Base + "/tags";
        }

        public static class Identity
        {
            // note: we are using verbs here, which is not standard for RESTful apis. Doing so for simplicity; we would usually not include these routes in the RESTful api
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
