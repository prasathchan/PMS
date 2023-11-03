using Microsoft.AspNetCore.Mvc;
using PMS_Library.Models.DataModel;
using Microsoft.EntityFrameworkCore;

namespace PMS_Api.Controllers.Doctor_Details
{
    public class DoctorsController(PMS_Entities context) : ControllerBase
    {
        private readonly PMS_Entities _context = context;

        [HttpGet]
        [Route("api/Doctors")]
        public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Doctor_Details>>> GetDoctors()
        {
            List<PMS_Library.Models.DataModel.Doctor_Details> cd = [];
            try
            {
                cd = await _context.Doctor_Details.ToListAsync();

            }
            catch
            {
                throw;
            }
            return cd;
        }

        [HttpGet]
        [Route("api/Doctors/{name}")]
        public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Doctor_Details>>> FetchDoctors_ByName(string name)
        {
            try
            {
                string cname = name.ToUpper();
                var cd = await _context.Doctor_Details.Where(x => x.DoctorName.Contains(cname)).ToListAsync();
                if (cd == null)
                {
                    return NotFound();
                }
                return cd;
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/Doctors/Add")]
        public async Task<ActionResult<PMS_Library.Models.DataModel.Doctor_Details>> PostDoctors([FromBody] PMS_Library.Models.DataModel.Doctor_Details cd)
        {
            try
            {
                var result = await _context.Doctor_Details.FirstOrDefaultAsync(x => x.DoctorName.ToUpper() == cd.DoctorName.ToUpper());
                if (result != null)
                {
                    return BadRequest("Doctor Already Exists");
                }
                else
                {
                    cd.DoctorID = "Doc-" + cd.DoctorName.ToLower() + Guid.NewGuid().ToString();
                    cd.HospitalID = cd.HospitalID;
                    cd.Date = DateTime.Now;
                    cd.DoctorName = cd.DoctorName.ToUpper();
                    cd.SpecialityCode =cd.SpecialityCode;
                    cd.Phone = cd.Phone;
                    cd.EmailAddress = cd.EmailAddress.ToLower();
                    cd.ResidentialAddress = cd.ResidentialAddress;
                    cd.JoiningDate = cd.JoiningDate;
                    cd.LoginEnabled = cd.LoginEnabled;
                    cd.Status = cd.Status;
                    _context.Doctor_Details.Add(cd);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
            return CreatedAtAction("FetchDoctors_ByName", new { name = cd.DoctorName }, cd);
        }
    }
}
