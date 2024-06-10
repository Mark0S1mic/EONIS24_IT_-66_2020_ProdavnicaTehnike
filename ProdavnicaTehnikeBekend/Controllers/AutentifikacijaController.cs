using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProdavnicaTehnikeBekend.Models;
using ProdavnicaTehnikeBekend.Models.DTOs;
using ProdavnicaTehnikeBekend.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProdavnicaTehnikeBekend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AutentifikacijaController : ControllerBase
    {
        private readonly IAutentifikacijaRepository _autentifikacijaRepository;
        private readonly HashingService _hashingService;

        public AutentifikacijaController(IAutentifikacijaRepository autentifikacijaRepository, HashingService hashingService)
        {
            _autentifikacijaRepository = autentifikacijaRepository;
            _hashingService = hashingService;

        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(LoginDto userLoginDto)
        {
            var result = await _autentifikacijaRepository.Authenticate(userLoginDto.Username, userLoginDto.Password);

            if (result == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(result);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Uklanjanje JWT tokena iz cookie-ja ili iz zaglavlja autorizacije
            Response.Cookies.Delete("AuthenticationCookie");
            Response.Headers.Remove("Authorization");

            return Ok(new { message = "Logout successful" });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // Proverite da li korisnik već postoji
            if (await _autentifikacijaRepository.UserExists(registerDto.Username))
                return BadRequest("Korisničko ime je već zauzeto");

            // Hashujte lozinku
            var hashedPassword = _hashingService.HashPassword(registerDto.Password);

            // Kreirajte novog korisnika
            var newUser = new Kupac
            {
                KorisnickoImeKupca = registerDto.Username,
                SifraKupca = hashedPassword,
                KontaktKupca = registerDto.email,          
         
            };

            await _autentifikacijaRepository.Register(newUser);

            return StatusCode(201);
        }



    }
}
