using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_Library.Models.DataModel;

namespace PMS_Api.Controllers.Doctor_Details
{
    public class DoctorSpecialityController(PMS_Entities context) : ControllerBase
    {
        private readonly PMS_Entities _context = context;

        [HttpGet]
        [Route("api/Doctors/Specialty")]
        public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Doctor_Specialty>>> GetDoctors()
        {
            List<PMS_Library.Models.DataModel.Doctor_Specialty> cd = [];
            try
            {
                cd = await _context.Doctor_Specialty.ToListAsync();

            }
            catch
            {
                throw;
            }
            return cd;
        }
    }
}
