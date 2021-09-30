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
using Brainchild.HMS.API.DTOs;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.Data;
using static Brainchild.HMS.Data.BookingService;

namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class BookingController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<BookingController> _logger;       
        public IBookingService _bookingService = new BookingService("Data Source=SNEHA;Initial Catalog=BrainchildHMS;Integrated Security=True;");

        public BookingController(BrainchildHMSDbContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Booking/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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


        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO booking)
        {
            int guestId = _bookingService.FindGuestByPhoneNumber(booking.Guest.GuestPhoneNo.ToString());
            List<Room> availableRooms = new List<Room>();
            if (guestId == 0)
            {
                _bookingService.CreateGuest(booking.Guest);
                guestId = _bookingService.FindGuestByPhoneNumber(booking.Guest.GuestPhoneNo);               
                availableRooms= _bookingService.CheckRoomAvailability(booking);                 
                if (availableRooms == null)
                {
                    //message there is no room available
                }
                else
                {
                    _bookingService.CreateBooking(guestId, booking);
                    int bookingId = _bookingService.GetBookingId();
                    booking.Rooms = availableRooms;
                    _bookingService.AddRoomBooking(bookingId,booking.Rooms[0].RoomId);
                }
            }
            else
            {
                availableRooms = _bookingService.CheckRoomAvailability(booking);
                if (availableRooms == null)
                {
                    //message there is no room available
                }
                else
                {
                    _bookingService.CreateBooking(guestId, booking);
                    int bookingId = _bookingService.GetBookingId();
                    booking.Rooms = availableRooms;
                    _bookingService.AddRoomBooking(bookingId, booking.Rooms[0].RoomId);
                }
            }

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
