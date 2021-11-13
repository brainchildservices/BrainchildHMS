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
using Microsoft.Extensions.Configuration;

namespace Brainchild.HMS.Web.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken]
    public class BillingController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<BillingController> _logger;
        private static IConfiguration _configuration;
        public IBillingService _billingService;
        public BillingController(BrainchildHMSDbContext context,ILogger<BillingController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _billingService = new BillingService(_configuration.GetConnectionString("DefaultConnection"));
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
            try
            {
                _logger.LogInformation("BillingController.CheckOut Method Called.");

                //Creating an object for BookingDTO
                BookingDTO booking = new BookingDTO();

                //fetching the booking details by using the bookingId.
                _logger.LogInformation($"_billingService.GetBookingDetails Method called with the parameter bookingId: {checkout.BookingId}");
                booking = _billingService.GetBookingDetails(checkout.BookingId);
                _logger.LogInformation($"_billingService.GetBookingDetails Method returned the booking details of bookingId: {checkout.BookingId}");

                //calculating the days spend in the hotel by using the check-in and checkout dates.
                _logger.LogInformation($"Calculating the number of days spend in the hotel by using {booking.CheckInDate} and {booking.CheckOutDate}");
                double noOfDaysSpend = (booking.CheckOutDate - booking.CheckInDate).TotalDays;
                _logger.LogInformation($"{noOfDaysSpend} days spend in the hotel.");

                //fetching the RoomRate by roomId
                _logger.LogInformation($"_billingService.GetRoomRateByRoomId Method called with parameters roomId: {checkout.RoomId} and hotelId: {checkout.HotelId}");
                double roomRate = _billingService.GetRoomRateByRoomId(checkout.RoomId, checkout.BookingId);
                _logger.LogInformation($"Calculated the Room Rate. roomRate: {roomRate}");

                //calculating the total room rate 
                _logger.LogInformation($"Calculating the totalRoomRate by using Room Rate({roomRate}) and Number of days Spend({noOfDaysSpend})");
                double totalRoomRate = roomRate * noOfDaysSpend;
                _logger.LogInformation($"Calculated the Total Room Rate. totalRoomRate: {totalRoomRate}");

                //fetch the total charges by the roomId
                _logger.LogInformation($"_billingService.GetTotalCharges Method called with parameter roomId: {checkout.RoomId}");
                double totalCharges = totalRoomRate + _billingService.GetTotalCharges(checkout.RoomId);
                _logger.LogInformation($"_billingService.GetTotalCharges Method returned the totalCharges({totalCharges})");

                //fetch the total payments
                _logger.LogInformation($"_billingService.GetTotalPayments Method called with parameter roomId: {checkout.RoomId}");
                double totalPayments = _billingService.GetTotalPayments(checkout.RoomId);
                _logger.LogInformation($"_billingService.GetTotalPayments Method returned the totalPayments({totalPayments})");

                //checking whether the totalcharges and totalpayments are equal
                if (totalCharges == totalPayments)
                {
                    //do checkout for the guest. (change the room status to available)
                    _logger.LogInformation($"_billingService.DoCheckOut Method called with parameter roomId:{checkout.RoomId}");
                    _billingService.DoCheckOut(checkout.RoomId);
                    _logger.LogInformation("Checked out the Guest by changing the room status to available");
                }
                else
                {
                    _logger.LogInformation($"Checking the totalCharges {totalCharges} and totalPayments {totalPayments}");
                    if (totalCharges > totalPayments)
                    {
                        _logger.LogInformation($"The totalCharges {totalCharges} is greater than totalPayments {totalPayments}");
                        return BadRequest($"{false}: Please pay the outstanding amount of Rs {totalCharges - totalPayments}/- for Check out the Guest");
                    }                      
                    else
                    {
                        _logger.LogInformation($"The totalCharges {totalCharges} is lesser than totalPayments {totalPayments}");
                        return BadRequest($"{false}: Please settle the amount of Rs {totalPayments - totalCharges}/- for Check out the Guest");
                    }
                        
                }
                
                return Ok($"{true} : Checked out the Guest");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
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
