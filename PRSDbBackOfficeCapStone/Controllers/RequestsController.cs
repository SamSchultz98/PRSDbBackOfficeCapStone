using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PRSDbBackOfficeCapStone.Models;

namespace PRSDbBackOfficeCapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }
        // GET: api/Requests/reviews/{userId}
        [HttpGet("reviews/{userid}")]
        public async Task<IEnumerable<Request>>? GetReviews(int userid)
        {
            List<Request> requests = await _context.Requests.ToListAsync();

            var filteredRequests =from request in requests
                                   join user in _context.Users on request.UserId equals user.Id
                                   where userid != request.UserId & request.Status == "Review"
                                   select request;
            return filteredRequests;
          
        }
        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // PUT: api/Requests/approve/{id}
        [HttpPut("approve/{id}")]
        public async Task<ActionResult<Request>> ForceApprove(int id)
        {
         Request? targ = await _context.Requests.FindAsync(id);
            if (targ is null)       //If the id doesn't match with one in the database
            {
                return NotFound();
            }
            targ.Status = "Approved";
            _context.Entry(targ).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var response= await _context.Requests.FindAsync(id);
            if(response is null)
            {
                return NotFound();
            }
            return response;
            
        }
        // PUT : api/Requests/deny/{id}
        [HttpPut("deny/{id}")]
        public async Task<ActionResult<Request>> ForceDeny(int id)
        {
            Request? targ = await _context.Requests.FindAsync(id);
            if (targ is null)       //If the id doesn't match with one in the database
            {
                return NotFound();
            }
            targ.Status = "DENIED";
            _context.Entry(targ).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var response = await _context.Requests.FindAsync(id);
            if(response is null)
            {
                return NotFound();
            }
            return response;

        }
        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
