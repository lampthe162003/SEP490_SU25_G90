using Microsoft.IdentityModel.Tokens;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SEP490_SU25_G90.vn.edu.fpt.Commons
{
    public class JwtTokenGenerator
    {
        private readonly string? _key;
        private readonly string? _issuer;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];

            if (string.IsNullOrEmpty(_key))
            {
                throw new InvalidOperationException("JWT key is missing from configuration");
            }

            if (string.IsNullOrEmpty(_issuer))
            {
                throw new InvalidOperationException("JWT issuer is missing from configuration");
            }
        }

        public string GenerateToken(LoginInformationResponse user, bool savePasswordCheck)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, user.UserRoles.First().Role.RoleName),
                new Claim("user_id", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: null,
                claims: claims,
                expires: savePasswordCheck ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
