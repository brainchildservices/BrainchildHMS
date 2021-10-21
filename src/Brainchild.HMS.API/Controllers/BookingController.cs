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
            List<Room> availableRooms = new List<Room>();

            //selecting the available rooms 
            availableRooms = _bookingService.GetAvailableRooms(booking);

            Hashtable availableRoomList = new Hashtable();

            //Converting availableRooms to Hashtable.
            foreach (var item in availableRooms)
            {
                availableRoomList.Add(item.RoomId,item.RoomNo);
            }

            //Checking the selected rooms are available.
            int count = 0;
            foreach (var item in booking.Rooms)
            {
                if (availableRoomList.ContainsValue(item.RoomNo))
                    count++;
            }                     

            if (booking.Rooms.Count != count)
            {                
                return BadRequest("The Selected rooms are NOT available on " + booking.CheckInDate.ToString("dd/MM/yyyy"));
            }
            else
            {
                GuestDTO guest = new GuestDTO();

                //Checking with the phone number,if the guest is already there fetching the details
                guest = _bookingService.FindGuestByPhoneNumber(booking.Guest.GuestPhoneNo.ToString());

                //if there is no existing data, Creating new Guest.
                if (guest.GuestId == 0)
                    guest.GuestId = _bookingService.CreateGuest(booking.Guest);

                //Creating the booking.
                int bookingId = _bookingService.CreateBooking(guest.GuestId, booking);            

                //Creating RoomBooking for the Guest.
                for(int i = 0; i < booking.Rooms.Count; i++)
                {
                    _bookingService.AddRoomBooking(bookingId, booking.Rooms[i].RoomId);
                }
               
            }


            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        //Check-in
        [HttpPost("{bookingId}/checkin")]
        public async Task<IActionResult> CheckIn(int bookingId, CheckInDTO checkIn)
        {
            try
            {
                _logger.LogInformation("BookingController.CheckIn Method Called.");

                //Created object for BookingDTO
                BookingDTO booking = new BookingDTO();
                //fetching the booking details
                _logger.LogInformation($"_bookingService.GetBookingDetails Method called with parameters BookingId: {bookingId}, HotelId:{checkIn.HotelId} and RoomNo: {checkIn.RoomNo}");
                booking = _bookingService.GetBookingDetails(bookingId, checkIn.HotelId, checkIn.RoomNo);
                _logger.LogInformation("Fetch the Booking details");

                //Checking the Booking is exists
                if (booking.BookingId != 0)
                {                    
                    //Checking the Booking is Cancelled or Not
                    if (booking.IsCancelled != 1)
                    {                       
                        //Checking the Check-in date with the current date
                        if (booking.CheckInDate == DateTime.Now)
                        {
                            //Checking the roomStatus for the room is already checked in or not
                            if (booking.RoomStatus==0)
                            {
                                //Doing the checkIn by changing the status of Rooms from vacant to occupied.
                                _logger.LogInformation($" _bookingService.DoCheckIn Method called with parameters RoomNo: {checkIn.RoomNo} and HotelId: {checkIn.HotelId}");
                                _bookingService.DoCheckIn(checkIn.RoomNo,checkIn.HotelId);
                                _logger.LogInformation("Done the CheckIn by changing the status of Room from vacant to occupied");

                                //Generate bill for the booking by room number
                                _logger.LogInformation($"_bookingService.GenerateBill Method called with parameters roomId :{booking.RoomId} and bookingId {bookingId}");
                                _bookingService.GenerateBill(booking.RoomId, bookingId);
                                _logger.LogInformation($"Generated bill for RoomNo:{checkIn.RoomNo}");
                            }
                            else
                            {
                                return BadRequest($"The RoomNo {checkIn.RoomNo} is already Checked-In");
                            }
                        }
                        else
                        {
                            return BadRequest("Check-In date is not Valid");
                        }
                    }
                    else
                    {
                        return BadRequest($"The booking for RoomNo {checkIn.RoomNo} is already Cancelled.");
                    }
                }
                else
                {
                    return BadRequest($"There is no Booking Available on the RoomNo:{checkIn.RoomNo}");
                }


                return NoContent();
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
