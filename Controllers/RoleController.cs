using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IndoorLocalization_API.Models;
using IndoorLocalization_API.Database;

namespace IndoorLocalization_API.Controllers
{
        [ApiController]

    [Route("[controller]")]
    public class RoleController : APIDatabaseContext<Role>
    {
        public RoleController(IndoorLocalizationContext context):base(context){}

        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<List<Role>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            if (roles == null)
            {
                return new List<Role>();
            }

            return roles;
        }

        [HttpGet]
        [Route("GetRoleById/{id}")]
        public async Task<Role> GetRoleById(int id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return new Role();
            }

            return role;
        }
    }
}
