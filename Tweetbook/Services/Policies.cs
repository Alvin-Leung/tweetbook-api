namespace Tweetbook.Services
{
    public static class Policies
    {
        public const string TagsPolicyName = "TagViewer";

        public static class CustomClaims
        {
            public const string TagsView = "tags.view";
        }
    }
}
