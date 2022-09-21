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
    public class RequestLinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestLinesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLines()
        {
            return await _context.RequestLines.ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        // PUT: api/RequestLines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await RecalculateRequestTotal(requestLine.RequestId);
            return NoContent();
        }

        // POST: api/RequestLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestLine.RequestId);
            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }


        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();
            await RecalculateRequestTotal(requestLine.RequestId);
            return NoContent();
        }
        private async Task<decimal> RecalculateRequestTotal(int requestid)
        {

            //List<Request> requestList = await _context.Requests.ToListAsync();
            //List<RequestLine> rLines = await _context.RequestLines.ToListAsync();
            Request? requestTarg = await _context.Requests.FindAsync(requestid);
            if (requestTarg is null)
            {
                throw new Exception("requestTarg is null");
            }
            //var currentTotal = requestTarg.Total;
            //Trying to do it all under one query 
            //var Target = from r in rLines                                                       //Getting Quantity from RequestLines
            //             join requests in _context.Requests on r.RequestId equals requests.Id   //Getting Total from requests
            //             join product in _context.Products on r.ProductId equals product.Id     //Getting price from products
            //             where requests.Id == requestid && r.RequestId == requestid
            //             select requests.Total=product.Price*r.Quantity;                        //Total = price * quantity

            //requestTarg.Total = Target.FirstOrDefault();
            var Linetotal = from r in _context.RequestLines
                            join request in _context.Requests on r.RequestId equals request.Id
                            join prod in _context.Products on r.ProductId equals prod.Id
                            where requestid == r.RequestId
                            select new
                            {
                                total = prod.Price * r.Quantity

                            };
            var Total = Linetotal.Sum(x => x.total);
            requestTarg.Total = Total;
            _context.Entry(requestTarg).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Total;
        }
        //    //Getting the price by joining the product table
        //    var price = from r in rLines
        //                join product in _context.Products on r.ProductId equals product.Id
        //                where r.RequestId == requestid
        //                select r.Product.Price;


        //    ////Getting each piece of the equation on their own
        //    //var price = from prod in _context.Products
        //    //             join r in rLines on prod.Id equals r.ProductId
        //    //             where r.RequestId == requestid
        //    //             select prod;

        //   //Was null?
        //    var quantity = from r in rLines
        //                   where r.RequestId == requestid
        //                   select r;
           
            
        //    var rPrice = price.FirstOrDefault();

        //    var rQuantity = quantity.FirstOrDefault();

        //    if (requestTarg is not null )
        //    {
        //        if (currentTotal > 0 && requestTarg.Id == requestid)
        //        {
        //            if (rQuantity is null)
        //            {
        //                requestTarg.Total = (rPrice * 0);
        //            }
        //            if (rQuantity is not null)
        //            {
                        
        //                requestTarg.Total = (currentTotal + (rPrice * rQuantity.Quantity));
        //            }
        //        }
        //        if (currentTotal == 0)
        //        {
                    
        //            if (rQuantity is not null)
        //            {
        //                requestTarg.Total = rPrice * rQuantity.Quantity;
        //            }
        //        }


        //    }
        //    _context.Entry(requestTarg).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    //Ok();
        //}

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }




    }
}
