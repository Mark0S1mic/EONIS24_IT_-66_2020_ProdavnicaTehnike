using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProdavnicaTehnikeBekend
{
    public class JwtHelper
    {

        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(int id, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            if (key.Length < 32)
            {
                throw new ArgumentOutOfRangeException(nameof(key), "The key size must be at least 256 bits (32 bytes).");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Role, role),
                // Add other claims if needed
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }

}

