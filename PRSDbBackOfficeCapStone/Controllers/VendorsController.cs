using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSDbBackOfficeCapStone.Models;

namespace PRSDbBackOfficeCapStone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VendorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // GET : api/endors/po/{vendorid}
        [HttpGet("po/{vendorId}")]
        public async Task<ActionResult<Po>> CreatePo(int vendorId)
        {
            var targVendor = await _context.Vendors.FindAsync(vendorId);
            

           var poLineQ = from v in _context.Vendors
                      join prod in _context.Products
                      on v.Id equals prod.VendorId   
                      join Rl in _context.RequestLines
                      on prod.Id equals Rl.ProductId
                      join R in _context.Requests
                      on Rl.RequestId equals R.Id
                      where R.Status == "APPROVED"
                      orderby Rl
                      select new
                      {
                          prodId= prod.Id,
                          Product = prod.Name,
                         Rl.Quantity,
                         prod.Price,
                          Linetotal = prod.Price * Rl.Quantity
                      };

            SortedList<int, Poline> sortLines = new SortedList<int, Poline>();
            foreach (var line in poLineQ)
            {
                if (!sortLines.ContainsKey(line.prodId))
                {
                    var poline = new Poline()
                    {
                        Product = line.Product,
                        Quantity = 0,
                        Price = line.Price,
                        LineTotal = line.Linetotal
                    };
                    sortLines.Add(line.prodId, poline);
                }

                sortLines[line.prodId].Quantity += line.Quantity;
            }
            var newPo = new Po();
            newPo.Vendor = targVendor;
            newPo.polines = sortLines.Values;
            newPo.PoTotal = sortLines.Values.Sum(x => x.LineTotal);
            return newPo;
            
            
            
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}
