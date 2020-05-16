using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Requests;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest userRegistrationRequest)
        {
            var authenticationResult = await this.identityService.RegisterAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);

            if (!authenticationResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authenticationResult.Errors
                });
            }

            return Ok(new AuthSuccessResponse { Token = authenticationResult.Token });
        }
    }
}
