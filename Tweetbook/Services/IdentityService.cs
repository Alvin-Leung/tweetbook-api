using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tweetbook.Domain;
using Tweetbook.Options;

namespace Tweetbook.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtSettings jwtSettings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult { Success = false, Errors = new[] { $"User with email {email} already exists." } };
            }

            var newUser = new IdentityUser()
            {
                UserName = email,
                Email = email
                // no need to calculate and populate the PasswordHash property ourselves. UserManager.CreateAsync will do this automatically.
            };

            var result = await userManager.CreateAsync(newUser, password);

            return new AuthenticationResult
            {
                Success = result.Succeeded,
                Token = result.Succeeded ? this.CreateToken(newUser) : null,
                Errors = result.Succeeded ? null : result.Errors.Select(x => x.Description)
            };
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new LoginResult { Success = false, Errors = new[] { $"User with email {email} does not exist." } };
            }

            var result = await userManager.CheckPasswordAsync(user, password);

            return new LoginResult
            {
                Success = result,
                Token = result ? this.CreateToken(user) : null
            };
        }

        private string CreateToken(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
