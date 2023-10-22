using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS_Library.Models.DataModel;
using Microsoft.EntityFrameworkCore;

namespace PMS_Api.Controllers.Client_Categories
{
	[ApiController]
	
	public class ClientCategoriesController(PMS_Entities context) : ControllerBase
	{
		private readonly PMS_Entities _context = context;

		[HttpGet]
		[Route("api/ClientCategories")]
		public async Task<ActionResult<IEnumerable<PMS_Library.Models.DataModel.Client_Categories>>> GetClient_Categories()
		{
			List<PMS_Library.Models.DataModel.Client_Categories> cc = [];
			try
			{
				cc = await _context.Client_Categories.ToListAsync();

			}
			catch
			{
                return BadRequest();
                throw;
			}
			return cc;
		}
	}
}
