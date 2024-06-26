using Microsoft.AspNetCore.Mvc;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using BCrypt.Net;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ProdavnicaTehnikeBekend.Repositories
{
    public class AutentifikacijaRepository : IAutentifikacijaRepository
    {
        private readonly ProdavnicaTehnikeContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly HashingService _hashingService;

        public AutentifikacijaRepository(ProdavnicaTehnikeContext dbContext, IConfiguration configuration, HashingService hashingService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _hashingService = hashingService;
        }

        public async Task<object> Authenticate(string username, string password)
        {
            var user = await _dbContext.Kupacs.SingleOrDefaultAsync(x => x.KorisnickoImeKupca == username);

            if (user != null && _hashingService.VerifyPassword(user.SifraKupca, password))
            {
                // Remove sensitive data
                user.SifraKupca = null;

                var token = GenerateJwtToken(user.KupacId, "User");
                return new { Token = token, Role = "User", KorisnickoImeKupca = user.KorisnickoImeKupca };
            }

            var admin = await _dbContext.Zaposlenis.SingleOrDefaultAsync(x => x.KorisnickoImeZaposlenog == username);

            if (admin != null && _hashingService.VerifyPassword(admin.SifraZaposlenog, password))
            {
                // Remove sensitive data
                admin.SifraZaposlenog = null;

                var token = GenerateJwtToken(admin.ZaposleniId, "Admin");
                return new { Token = token, Role = "Admin" };
            }

            return null;
        }

       public async Task Register(Kupac kupac)
        {
            await _dbContext.Kupacs.AddAsync(kupac);
            await _dbContext.SaveChangesAsync();
        }

        

        public async Task<bool> UserExists(string username)
        {
            return await _dbContext.Kupacs.AnyAsync(u => u.KorisnickoImeKupca == username);
        }




        private string GenerateJwtToken(int id, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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
