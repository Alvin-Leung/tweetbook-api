using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Controllers.V1.Requests
{
    public class UserLoginRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
