using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SWD392_AffiliLinker.Services.Services
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<User> _userManager;

		public JwtTokenService(IConfiguration configuration, UserManager<User> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
		}
		public async Task<string> GenerateJwtToken(User user)
		{
			var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]) ?? throw new Exception("JWT_KEY is not set");

			var fullName = user.FirstName + " " + user.LastName;

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim("FullName", fullName),
				new Claim("Avatar", user.Avatar ?? ""),
            };

			IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
			foreach (string role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(1),
				Issuer = _configuration["JWT:ValidIssuer"] ?? throw new Exception("JWT_ISSUER is not set"),
				Audience = _configuration["JWT:ValidAudience"] ?? throw new Exception("JWT_AUDIENCE is not set"),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
			};
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return (tokenHandler.WriteToken(token));
		}

        public async Task<string> GenerateRefreshToken(User user)
        {
            string? refreshToken = Guid.NewGuid().ToString();

            string? initToken = await _userManager.GetAuthenticationTokenAsync(user, "Default", "RefreshToken");
            if (initToken != null)
            {

                await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

            }

            await _userManager.SetAuthenticationTokenAsync(user, "Default", "RefreshToken", refreshToken);
            return refreshToken;
        }
    }
}
