using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Brainchild.HMS.Core.Models;
using Brainchild.HMS.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections;
using Brainchild.HMS.Data;
using Brainchild.HMS.Data.DTOs;

namespace Brainchild.HMS.Web.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class BillingController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<BillingController> _logger;
        public IBillingService _billingService = new BillingService("Data Source=SNEHA;Initial Catalog=BrainchildHMS;Integrated Security=True;");
        public BillingController(BrainchildHMSDbContext context,ILogger<BillingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Billing
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Billing>>> GetBillings()
        {
            return await _context.Billings.ToListAsync();
        }

        // GET: api/Billing/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBilling(int id)
        {
            var billing = await _context.Billings.FindAsync(id);

            if (billing == null)
            {
                return NotFound();
            }

            return billing;
        }

        // PUT: api/Billing/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBilling(int id, Billing billing)
        {
            if (id != billing.BillingId)
            {
                return BadRequest();
            }

            _context.Entry(billing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillingExists(id))
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

        // POST: api/Billing
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Billing>> CheckOut(CheckOutDTO checkout)
        {
            BookingDTO booking = new BookingDTO();

            //fetching the booking details by using the bookingId.
            booking = _billingService.GetBookingDetails(checkout.BookingId);

            //calculating the days spend in the hotel by using the check-in and checkout dates.
            double noOfDaysSpend = (booking.CheckOutDate - booking.CheckInDate).TotalDays;

            //fetching the RoomRate by roomId
            double roomRate = _billingService.GetRoomRateByRoomId(checkout.RoomId, checkout.HotelId);

            //calculating the total room rate 
            double totalRoomRate = roomRate * noOfDaysSpend;



            return CreatedAtAction("GetBilling", new { id = checkout.BookingId }, checkout);
        }

        // DELETE: api/Billing/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBilling(int id)
        {
            var billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            _context.Billings.Remove(billing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillingExists(int id)
        {
            return _context.Billings.Any(e => e.BillingId == id);
        }
    }
}
