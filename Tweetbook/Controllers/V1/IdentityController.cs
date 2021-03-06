﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest userRegistrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = this.ModelState.Values.SelectMany(state => state.Errors.Select(error => error.ErrorMessage))
                });
            }

            var authenticationResult = await this.identityService.RegisterAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);

            if (!authenticationResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authenticationResult.Errors
                });
            }

            return Ok(new AuthSuccessResponse { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken });
        }

        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var authenticationResult = await this.identityService.RefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken);

            if (!authenticationResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authenticationResult.Errors
                });
            }

            return Ok(new AuthSuccessResponse { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userRegistrationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = this.ModelState.Values.SelectMany(state => state.Errors.Select(error => error.ErrorMessage))
                });
            }

            var authenticationResult = await this.identityService.LoginAsync(userRegistrationRequest.Email, userRegistrationRequest.Password);

            if (!authenticationResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    Errors = authenticationResult.Errors
                });
            }

            return Ok(new AuthSuccessResponse { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken });
        }
    }
}
