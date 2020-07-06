namespace Tweetbook.Services
{
    public static class Policies
    {
        public const string TagsPolicyName = "TagViewer";

        /// <summary>
        /// Custom claim types
        /// </summary>
        /// <remarks>
        /// Remember that claim types are added to JWTs, which are part of every request requiring JWT authorization.
        /// As such, aim to minimize the string size of claim types; for example, JWT types included by default are 
        /// typically abbreviated 2-3 letter strings (id, nbf, exp, iat, etc.)
        /// </remarks>
        public static class ClaimTypes
        {
            public const string TagsView = "tags.view";
        }
    }
}
