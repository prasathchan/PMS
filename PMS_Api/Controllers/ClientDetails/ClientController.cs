using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_Library.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace PMS_Api.Controllers.ClientDetails
{
	[Route("api/[controller]")]
	[ApiController]
	public class ClientController : ControllerBase
	{
		private readonly PMS_Entities _context;

		public ClientController(PMS_Entities context)
		{
			_context = context;
		}

		//GET: api/Client
	   [HttpGet]
		public async Task<ActionResult<IEnumerable<Client_Details>>> GetClients()
		{
			return await _context.Client_Details.ToListAsync();
		}
	}
}
