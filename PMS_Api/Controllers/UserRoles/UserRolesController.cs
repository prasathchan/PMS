using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_Library.Models.DataModel;

namespace PMS_Api.Controllers.UserRoles
{

    [ApiController]
    public class UserRolesController(PMS_Entities context) : ControllerBase
    {
        private readonly PMS_Entities _context = context;

        // GET: api/UserRoles
        [HttpGet]
        [Route("api/UserRoles")]
        public async Task<ActionResult<IEnumerable<User_Roles>>> GetUserRoles()
        {
            List<User_Roles> ur = [];
            try
            {
                ur = await _context.User_Roles.ToListAsync();
                
            }
            catch
            {
                throw;
            }
            return ur;
        }

        [HttpGet]
        [Route("api/UserRoles/{rid}")]
        public async Task<ActionResult<User_Roles>> GetUser_Roles(string rid)
        {
            var ur = await _context.User_Roles.FindAsync(rid);
            if (ur == null)
            {
                return NotFound();
            }
            return ur;
        }
    }
}
