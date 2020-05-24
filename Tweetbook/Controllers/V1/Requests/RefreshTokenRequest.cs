namespace Tweetbook.Controllers.V1.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
