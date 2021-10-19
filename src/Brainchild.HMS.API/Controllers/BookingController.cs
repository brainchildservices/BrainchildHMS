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
using Brainchild.HMS.Data.DTOs;
using System.Data;
using System.Data.SqlClient;
using Brainchild.HMS.Data;
using static Brainchild.HMS.Data.BookingService;
using System.Collections;

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
            try
            {
                _logger.LogInformation("BookingController.PostBooking Method Called.");

                List<Room> availableRooms = new List<Room>();

                //selecting the available rooms 
                _logger.LogInformation($"_bookingService.GetAvailableRooms Method called with Parameters {booking.BookingId},{booking.HotelId},{booking.CheckInDate},{booking.CheckOutDate}");
                availableRooms = _bookingService.GetAvailableRooms(booking);
                _logger.LogInformation($"Fetched the available rooms on {booking.CheckInDate} and {booking.CheckOutDate} Dates");

                //Created hashtable
                Hashtable availableRoomList = new Hashtable();

                //Converting availableRooms to Hashtable.
                _logger.LogInformation("Adding the values to the HashTable");
                foreach (var item in availableRooms)
                {                    
                    availableRoomList.Add(item.RoomId, item.RoomNo);
                    _logger.LogInformation($"Added the values {item.RoomId} and {item.RoomNo} to the HashTable");
                }

                //Checking the selected rooms are available.
                int count = 0;
                _logger.LogInformation("Checking the selected rooms are available");
                foreach (var item in booking.Rooms)
                {
                    if (availableRoomList.ContainsValue(item.RoomNo))
                    {
                        count++;
                        _logger.LogInformation($"The selected roomNo: {item.RoomNo} is available. The count incremented to {count}");
                    }
                        
                }

                if (booking.Rooms.Count != count)
                {
                    _logger.LogInformation("The selected Rooms are NOT Available");
                    return BadRequest("The Selected rooms are NOT available on " + booking.CheckInDate.ToString("dd/MM/yyyy"));
                }
                else
                {
                    GuestDTO guest = new GuestDTO();

                    //Checking with the phone number,if the guest is already there fetching the details
                    _logger.LogInformation($"_bookingService.FindGuestByPhoneNumber Method called with Parameter GuestPhoneNo {booking.Guest.GuestPhoneNo}");
                    guest = _bookingService.FindGuestByPhoneNumber(booking.Guest.GuestPhoneNo.ToString());

                    //if there is no existing data, Creating new Guest.
                    if (guest.GuestId == 0)
                    {
                        _logger.LogInformation($"_bookingService.CreateGuest Method called with Parameters {booking.Guest.GuestName}, {booking.Guest.GuestPhoneNo}, {booking.Guest.GuestEmail}, {booking.Guest.GuestCountry}, {booking.Guest.GuestAddress}");
                        guest.GuestId = _bookingService.CreateGuest(booking.Guest);
                        _logger.LogInformation($"Created Guest and returned the guestId: {guest.GuestId}");
                    }


                    //Creating the booking.
                    _logger.LogInformation($"_bookingService.CreateBooking Method called with Parameters {guest.GuestId}, {booking.NoOfAChildren}, {booking.NoOfAdults}, {booking.CheckInDate}, {booking.CheckOutDate}, {booking.HotelId}");
                    int bookingId = _bookingService.CreateBooking(guest.GuestId, booking);
                    _logger.LogInformation($"Created Booking and returned the bookingId: {bookingId}");

                    //Creating RoomBooking for the Guest.
                    for (int i = 0; i < booking.Rooms.Count; i++)
                    {
                        _logger.LogInformation($"_bookingService.AddRoomBooking Method called with Parameters bookingId: {bookingId} and roomId: {booking.Rooms[i].RoomId}");
                        _bookingService.AddRoomBooking(bookingId, booking.Rooms[i].RoomId);
                        _logger.LogInformation("Creaetd Room Bookings");
                    }
                }

                return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
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
