using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PruebaFinanzautos.Models;
using System.Security.Cryptography;
using PruebaFinanzautos.Data;

namespace PruebaFinanzautos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly PruebaFinanzautosContext _context;

        public AutenticacionController(IConfiguration config, PruebaFinanzautosContext context)
        {
            _secretKey = config.GetSection("settings").GetSection("secretKey").Value;
            _context = context;
        }

        [HttpPost]
        [Route("Validar")]
        public async Task<IActionResult> Validar([FromBody] Usuario request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.ClaveHash))
            {
                return BadRequest("Invalid request.");
            }

            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == request.Correo);

            if (user == null || !VerifyPassword(request.ClaveHash, user.ClaveHash))
            {
                return Unauthorized();
            }

            var keyBytes = Encoding.ASCII.GetBytes(_secretKey);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Correo));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { token = tokenString });
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] Usuario request)
        {
            if (request == null || string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.ClaveHash))
            {
                return BadRequest("Invalid request.");
            }

            if (await _context.Usuarios.AnyAsync(u => u.Correo == request.Correo))
            {
                return Conflict("User already exists.");
            }

            var passwordHash = HashPassword(request.ClaveHash);

            var user = new Usuario
            {
                Correo = request.Correo,
                ClaveHash = passwordHash
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Validar), new { correo = request.Correo }, user);
        }

        private string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            var saltBase64 = Convert.ToBase64String(salt);
            var hashBase64 = Convert.ToBase64String(hash);
            return $"{saltBase64}${hashBase64}";
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('$');
            if (parts.Length != 2)
                return false;

            var salt = Convert.FromBase64String(parts[0]);
            var storedHashBytes = Convert.FromBase64String(parts[1]);

            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            return CryptographicOperations.FixedTimeEquals(hash, storedHashBytes);
        }
    }
}
