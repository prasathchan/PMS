using Microsoft.AspNetCore.Mvc;
using PMS_Library.Models.DataModel;
using Microsoft.EntityFrameworkCore;
using PMS_Library.Models.CustomModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
        public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Doctor_Details>>> FetchDoctorsByName(string name)
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

        [HttpGet]
        [Route("api/Doctors/email/{email}")]
        public async Task<ActionResult<PMS_Library.Models.DataModel.Doctor_Details>> FetchDoctorsByEmail(string email)
        {
            try
            {
                string mail = email.ToUpper();
                var cd = await _context.Doctor_Details.Where(x => x.EmailAddress.Contains(mail)).FirstOrDefaultAsync();
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


		[HttpGet]
		[Route("api/Doctors/hospital/{hospitalid}")]
		public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Doctor_Details>>> FetchDoctorsByHospital(string hospitalid)
		{
			try
			{
				var cd = await _context.Doctor_Details.Where(x => x.HospitalID.Contains(hospitalid)).ToListAsync();
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
                var result = await _context.Doctor_Details.FirstOrDefaultAsync(x => x.DoctorName.Equals(cd.DoctorName.ToUpper()));
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
                    cd.EmailAddress = cd.EmailAddress.ToUpper();
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
            return CreatedAtAction("FetchDoctorsByName", new { name = cd.DoctorName }, cd);
        }

        [HttpPut]
        [Route("api/Doctors/Edit")]
        public async Task<ActionResult<PMS_Library.Models.DataModel.Doctor_Details>> PutDoctors([FromBody] PMS_Library.Models.DataModel.Doctor_Details cd)
        {
			try
            {
				var result = await _context.Doctor_Details.FirstOrDefaultAsync(x => (x.DoctorName+x.EmailAddress).Equals(cd.DoctorName+cd.EmailAddress.ToUpper()));
				if (result == null)
                {
					return NotFound();
				}
				else
                {
					result.DoctorName = cd.DoctorName.ToUpper();
					result.SpecialityCode = cd.SpecialityCode;
					result.Phone = cd.Phone;
					result.EmailAddress = cd.EmailAddress.ToUpper();
					result.ResidentialAddress = cd.ResidentialAddress;
					result.JoiningDate = cd.JoiningDate;
					result.LoginEnabled = cd.LoginEnabled;
					result.Status = cd.Status;
					await _context.SaveChangesAsync();
				}
			}
			catch
            {
				throw;
			}
			return CreatedAtAction("FetchDoctorsByName", new { name = cd.DoctorName }, cd);
		}

        [HttpDelete]
        [Route("api/Doctors/Delete")]
        public async Task<ActionResult<PMS_Library.Models.DataModel.Doctor_Details>> DeleteDoctors([FromBody()]DeleteDoctor dd)
        {
			try
            {
				var cd = await _context.Doctor_Details.Where(x => (x.DoctorName+x.EmailAddress).Equals(dd.DoctorName.ToUpper()+dd.EmailAddress.ToUpper())).FirstOrDefaultAsync();
				if (cd == null)
                {
					return NotFound();
				}
				_context.Doctor_Details.Remove(cd);
				await _context.SaveChangesAsync();
				return cd;
			}
			catch
            {
				throw;
			}
        }
    }
}
