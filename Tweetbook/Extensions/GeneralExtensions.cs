using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Tweetbook.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(claims => claims.Type == Services.IdentityService.UserIdClaimType).Value;
        }
    }
}
