using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWebApplication.Data;
using MovieWebApplication.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieWebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly string _jwtSecret;

        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _jwtSecret = config["Jwt:SecretKey"];
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email already exists");

            var salt = GenerateSalt();
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Salt = salt,
                PasswordHash = HashPassword(request.Password, salt),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Простой токен в формате Base64
            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{user.Id}:{user.Username}:{DateTime.UtcNow.Ticks}")
            );

            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.PasswordHash != HashPassword(request.Password, user.Salt))
                return Unauthorized("Invalid credentials");

            var token = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{user.Id}:{user.Username}:{DateTime.UtcNow.Ticks}")
            );

            return Ok(new { Token = token });
        }

        private string GenerateSalt()
        {
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return Convert.ToBase64String(bytes);
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}