using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using IndoorLocalization_API.Database;

namespace IndoorLocalization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : APIDatabaseContext<User>
    {

        public UserController(IndoorLocalizationContext context) : base(context) { }

        [HttpGet("Profile/{id}")]
        public async Task<ActionResult<User>> GetUserProfile(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel model)
        {
            if (model == null)
                return BadRequest("Invalid user data.");

            if (_context.Users.Any(u => u.Username == model.Username))
                return BadRequest("Username already exists.");

            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Name, username, and password cannot be empty.");

            var user = new User
            {
                Name = model.Name,
                Username = model.Username,
                Email = model.Email,
                RoleId = 2
            };

            var salt = Guid.NewGuid().ToString();
            user.Salt = salt;
            user.Password = HashPassword(model.Password, salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginModel model)
        {
            if (model == null)
                return BadRequest("Invalid login data.");

            var user = await _context.Users
                                     .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            var hashedPassword = HashPassword(model.Password, user.Salt);
            if (hashedPassword != user.Password)
                return Unauthorized("Invalid username or password.");

            var response = new LoginResponse
            {
                Id = user.Id,
                Name = user.Name,
                Username = user.Username,
                RoleId = user.RoleId ?? 0,
            };

            return Ok(response);
        }

        private string HashPassword(string password, string salt)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public class RegisterModel
        {
            public string Name { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class LoginResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Username { get; set; }
            public int RoleId { get; set; }
        }
    }
}
