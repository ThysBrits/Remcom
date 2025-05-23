using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Remcom.Api.Data;
using Remcom.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Remcom.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly RemcomDbContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(RemcomDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || user.IsLocked)
                return Unauthorized("Invalid credentials or account locked.");

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                    user.IsLocked = true;
                await _context.SaveChangesAsync();
                return Unauthorized("Invalid credentials.");
            }

            user.FailedLoginAttempts = 0;
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return Ok(new { user.Id, user.Username, user.Role, token });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return BadRequest("Username already exists.");
            var user = new User
            {
                Username = request.Username,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                IsLocked = false,
                FailedLoginAttempts = 0
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { user.Id, user.Username, user.Role });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            user.Role = request.Role;
            if (!string.IsNullOrWhiteSpace(request.Password))
                user.PasswordHash = HashPassword(request.Password);
            user.IsLocked = request.IsLocked;
            await _context.SaveChangesAsync();
            return Ok(new { user.Id, user.Username, user.Role, user.IsLocked });
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var keyString = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
    public class CreateUserRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public UserRole Role { get; set; }
    }
    public class UpdateUserRequest
    {
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public bool IsLocked { get; set; }
    }
}
