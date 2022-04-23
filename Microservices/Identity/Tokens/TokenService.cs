using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Tokens
{
    internal interface ITokenService
    {
        string BuildToken(string key, string issuer, IEnumerable<string> audience, string userName, String userId, TimeSpan duration);
    }

    public class TokenService : ITokenService
    {
        public string BuildToken(string key, string issuer, IEnumerable<string> audience, string userName, String userId, TimeSpan duration)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Sub, userId),
            };

            claims.AddRange(audience.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: DateTime.Now.Add(duration),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
