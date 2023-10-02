using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_Library.Models.DataModels;

namespace PMS_Api.Controllers.ClientDetails
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class Client_DetailsController : ControllerBase
    //{
    //    private readonly PMS_ClientDetailsDbContext _context;

    //    public Client_DetailsController(PMS_ClientDetailsDbContext context)
    //    {
    //        _context = context;
    //    }

    //    // GET: api/Client_Details
    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<Client_Details>>> GetClient_Details()
    //    {
    //        return await _context.Client_Details.ToListAsync();
    //    }

    //    // GET: api/Client_Details/5
    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<Client_Details>> GetClient_Details(string id)
    //    {
    //        var client_Details = await _context.Client_Details.FindAsync(id);

    //        if (client_Details == null)
    //        {
    //            return NotFound();
    //        }

    //        return client_Details;
    //    }

    //    // PUT: api/Client_Details/5
    //    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> PutClient_Details(string id, Client_Details client_Details)
    //    {
    //        if (id != client_Details.ClientID)
    //        {
    //            return BadRequest();
    //        }

    //        _context.Entry(client_Details).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!Client_DetailsExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }

    //    // POST: api/Client_Details
    //    // To protect from over posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<Client_Details>> PostClient_Details(Client_Details client_Details)
    //    {
    //        _context.Client_Details.Add(client_Details);
    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateException)
    //        {
    //            if (Client_DetailsExists(client_Details.ClientID))
    //            {
    //                return Conflict();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return CreatedAtAction("GetClient_Details", new { id = client_Details.ClientID }, client_Details);
    //    }

    //    // DELETE: api/Client_Details/5
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteClient_Details(string id)
    //    {
    //        var client_Details = await _context.Client_Details.FindAsync(id);
    //        if (client_Details == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.Client_Details.Remove(client_Details);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool Client_DetailsExists(string id)
    //    {
    //        return _context.Client_Details.Any(e => e.ClientID == id);
    //    }
    //}
}
