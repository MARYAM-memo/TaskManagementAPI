using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoAPI.Interfaces;
using TodoAPI.Settings;

namespace TodoAPI.Services;

public class TokenServices(IOptions<JwtSettings> options, UserManager<IdentityUser<int>> userManager) : ITokenServices
{
      readonly JwtSettings jwt = options.Value;
      readonly UserManager<IdentityUser<int>> userMng = userManager;

      public async Task<string> GenerateAccessToken(IdentityUser<int> user)
      {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt.Secret);
            var roles = await userMng.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                  new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                  new(JwtRegisteredClaimNames.Email, user.Email??""),
                  new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                  Subject = new ClaimsIdentity(claims),
                  Expires = DateTime.UtcNow.AddMinutes(jwt.AccessTokenExpiryMinutes),
                  Issuer = jwt.Issuer,
                  Audience = jwt.Audience,
                  SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                  ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
      }
}
