using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD392_AffiliLinker.Repositories.Entities;
using SWD392_AffiliLinker.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
			byte[] key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]) ?? throw new Exception("JWT_KEY is not set");
			List<Claim> claims = new List<Claim>
			{
				new(ClaimTypes.NameIdentifier,user.Id.ToString()),
				new(ClaimTypes.UserData, user.Avatar.ToString()),
				new Claim(ClaimTypes.Email, user.Email)
			};
			IEnumerable<string> roles = _userManager.GetRolesAsync(user: user).Result;
			foreach (string role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddHours(1),
				Issuer = _configuration["JWT:ValidIssuer"] ?? throw new Exception("JWT_ISSUER is not set"),
				Audience = _configuration["JWT:ValidAudience"] ?? throw new Exception("JWT_AUDIENCE is not set"),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return (tokenHandler.WriteToken(token));
		}
	}
}
