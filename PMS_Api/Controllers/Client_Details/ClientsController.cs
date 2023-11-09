using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_Library.Models.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace PMS_Api.Controllers.ClientDetails
{
	public class ClientsController(PMS_Entities context) : ControllerBase
	{
		private readonly PMS_Entities _context = context;


		[HttpGet]
		[Route("api/Clients")]
		public async Task<ActionResult<IEnumerable<Client_Details>>> GetClients()
		{
			List<Client_Details> cd = [];
			try
			{
				cd = await _context.Client_Details.ToListAsync();
				
			}
			catch
			{
				throw;
			}
			return cd;
		}


		[HttpGet]
		[Route("api/Clients/{name}")]
		public async Task<ActionResult<Client_Details>> FetchClientDetailsByName(string name)
		{
			try
			{
                string cname = name.ToUpper();
                var cd = await _context.Client_Details.Where(x => x.ClientName.Contains(cname)).FirstOrDefaultAsync();
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
        [Route("api/Clients/email/{email}")]
        public async Task<ActionResult<Client_Details>> FetchClientDetailsByEmail(string email)
        {
			try
			{
                var cd = await _context.Client_Details.Where(x => x.EmailID.Contains(email.ToUpper())).FirstOrDefaultAsync();
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
		[Route("api/Clients/{name}/{category}")]
		public async Task<ActionResult<Client_Details>> GetClient_Details(string name, string category)
		{
			
			var cd = await _context.Client_Details.FirstOrDefaultAsync(x => x.ClientName + x.Category ==  name.ToUpper() + category.ToUpper());
			if (cd == null)
			{
				return NotFound();
			}
			return cd;
		}

		
		[HttpPut]
		[Route("api/Clients/Update")]
        public async Task<IActionResult> PutClient_Details([FromBody]Client_Details cd)
		{
			var result = await _context.Client_Details.FirstOrDefaultAsync(x => x.ClientName.ToUpper() + x.Category == cd.ClientName.ToUpper() + cd.Category);
			if (result != null)
			{			
				try
				{
					if(cd.Phone != null && cd.Phone != string.Empty && cd.Phone != "")
					{
						result.Phone = cd.Phone;
					}
					if (cd.EmailID != null && cd.EmailID != string.Empty && cd.EmailID != "")
					{
						result.EmailID = cd.EmailID;
					}
					result.DateCreated = result.DateCreated;
					result.LastModified = DateTime.Now;
					_context.Entry(result).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					throw;
				}
			}
			else
			{
				return NotFound();
			}
			return CreatedAtAction("FetchClientDetailsByName", new { name = result.ClientName }, cd);
		}

		[HttpPost]
		[Route("api/Clients/Add")]
		public async Task<ActionResult<Client_Details>> PostClient_Details([FromBody]Client_Details cd)
		{
			try
			{
				var result = await _context.Client_Details.FirstOrDefaultAsync(x => x.ClientName.ToUpper() + x.Category == cd.ClientName.ToUpper() + cd.Category);
				if(result != null)
				{
					return BadRequest();
				}
				else
				{
					cd.HospitalID = "cli-"+ cd.ClientName.ToLower() + Guid.NewGuid().ToString();
					cd.ClientName = cd.ClientName.ToUpper();
					cd.Category = cd.Category.ToUpper();
					cd.DateCreated = DateTime.Now;
					cd.LastModified = DateTime.Now;
					cd.EmailID	= cd.EmailID.ToLower();
					cd.Phone=cd.Phone;
					_context.Client_Details.Add(cd);
					await _context.SaveChangesAsync();
				}
			}
			catch
			{
				throw;
			}
			return CreatedAtAction("GetClients", new { id = cd.HospitalID }, cd);
		}

		[HttpDelete]
		[Route("api/Clients/{name}")]
		public async Task<IActionResult> DeleteClient_DetailsByName(string name)
		{
			try
			{
                var cd = await _context.Client_Details.FirstOrDefaultAsync(x => x.ClientName.ToUpper().Equals(name.ToUpper()));
				if (cd == null)
				{
					return NotFound();
				}
				_context.Client_Details.Remove(cd);
				await _context.SaveChangesAsync();
				return NoContent();
			}
			catch(DbUpdateConcurrencyException)
			{
				throw;

			}
		}
	
	}
}