using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace IndoorLocalization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IndoorLocalizationContext _context;

        public UserController(IndoorLocalizationContext context)
        {
            _context = context;
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
                RoleId = 2 
            };

            var salt = Guid.NewGuid().ToString();
            user.Salt = salt;
            user.Password = HashPassword(model.Password, salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
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
        }
    }
}
