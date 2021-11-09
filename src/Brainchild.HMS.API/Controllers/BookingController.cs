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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class BookingController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<BookingController> _logger;
        private static IConfiguration _configuration;
        public IBookingService _bookingService;

        public BookingController(BrainchildHMSDbContext context, ILogger<BookingController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _bookingService = new BookingService(_configuration.GetConnectionString("DefaultConnection"));
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

                List<RoomDTO> availableRooms = new List<RoomDTO>();
                Booking bookings = new Booking();
                int bookingId = 0;

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
                    _logger.LogInformation($"_bookingService.CreateBooking Method called with Parameters {guest.GuestId}, {booking.NoOfChildren}, {booking.NoOfAdults}, {booking.CheckInDate}, {booking.CheckOutDate}, {booking.HotelId}");
                    bookingId = _bookingService.CreateBooking(guest.GuestId, booking);
                    _logger.LogInformation($"Created Booking and returned the bookingId: {bookingId}");

                    //Creating RoomBooking for the Guest.
                    for (int i = 0; i < booking.Rooms.Count; i++)
                    {
                        //Checking RoomRate passed or Not
                        if (booking.Rooms[i].RoomRate == 0)
                        {
                            //Fetching the roomrate 
                            _logger.LogInformation($"_bookingService.GetRoomRate Method called with parameters RoomId:{booking.Rooms[i].RoomId} and HotelId: {booking.HotelId}");
                            booking.Rooms[i].RoomRate = _bookingService.GetRoomRate(booking.Rooms[i].RoomId, booking.HotelId);
                            _logger.LogInformation($"Fetched the RoomRate:{booking.Rooms[i].RoomRate}");
                        }                        
                        _logger.LogInformation($"_bookingService.AddRoomBooking Method called with Parameters bookingId: {bookingId} and roomId: {booking.Rooms[i].RoomId}");
                        _bookingService.AddRoomBooking(bookingId, booking.Rooms[i].RoomId, booking.Rooms[i].RoomRate);
                        _logger.LogInformation("Created Room Bookings");

                    }

                    bookings = _bookingService.GetBooking(bookingId);
                    
                }
                string jsonValue = JsonConvert.SerializeObject(bookings);
                _logger.LogInformation($"Returned available Rooms Lists: {jsonValue}");

                return Ok(bookings);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{bookingId}/cancelbooking")]
        public async Task<IActionResult> CancelBooking(int bookingId, CancelBookingDTO cancelBooking)
        {
            try
            {
                _logger.LogInformation("BookingController.CancelBooking Method Called.");

                //Cancel the booking by changing the status
                _logger.LogInformation($"_bookingService.CancelBooking Method Called with Parameter bookingId({bookingId})");
                _bookingService.CancelBooking(bookingId);
                _logger.LogInformation($"Cancelled the Booking(bookingId-{bookingId}");

                //Add Cancel Notes
                _logger.LogInformation($"_bookingService.AddCancelNotes Method called with parameters{bookingId} and {cancelBooking.NoteDescription}");
                _bookingService.AddCancelNotes(bookingId, cancelBooking.NoteDescription);
                _logger.LogInformation("Added the Notes for Cancellation");

                //Deleting the RoomBookings by BookingId
                _logger.LogInformation($"_bookingService.DeleteRoomBookings Method called with parameters{bookingId}");
                _bookingService.DeleteRoomBookings(bookingId);
                _logger.LogInformation($"Deleted all the RoomBookings on the BookingId - {bookingId}");


                return Ok($"Cancelled the Booking. BookingId:{bookingId}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

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
                if (booking != null)
                {
                    _logger.LogInformation("Booking existing");
                    //Checking the Booking is Cancelled or Not
                    if (booking.IsCancelled != 1)
                    {
                        _logger.LogInformation("Booking is not Cancelled");
                        //Checking the Check-in date with the current date
                        if (booking.CheckInDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                        {
                            _logger.LogInformation("Check-In date is Valid");
                            //Checking the roomStatus for the room is already checked in or not
                            if (booking.RoomStatus == RoomStatus.Vacant)
                            {
                                _logger.LogInformation("Room status is Vacant");
                                //Doing the checkIn by changing the status of Rooms from vacant to occupied.
                                _logger.LogInformation($" _bookingService.DoCheckIn Method called with parameters RoomNo: {checkIn.RoomNo} and HotelId: {checkIn.HotelId}");
                                _bookingService.DoCheckIn(checkIn.RoomNo, checkIn.HotelId);
                                _logger.LogInformation("Done the CheckIn by changing the status of Room from vacant to occupied");

                                //Generate bill for the booking by room number
                                _logger.LogInformation($"_bookingService.GenerateBill Method called with parameters roomId :{booking.RoomId} and bookingId {bookingId}");
                                _bookingService.GenerateBill(booking.RoomId, bookingId);
                                _logger.LogInformation($"Generated bill for RoomNo:{checkIn.RoomNo}");
                            }
                            else
                            {
                                _logger.LogInformation($"The RoomNo {checkIn.RoomNo} is already Checked-In");
                                return BadRequest($"The RoomNo {checkIn.RoomNo} is already Checked-In");
                            }
                        }
                        else
                        {
                            _logger.LogInformation("Check-In date is not Valid");
                            return BadRequest("Check-In date is not Valid");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"The booking for RoomNo {checkIn.RoomNo} is already Cancelled.");
                        return BadRequest($"The booking for RoomNo {checkIn.RoomNo} is already Cancelled.");
                    }
                }
                else
                {
                    _logger.LogInformation($"There is no Booking Available on the RoomNo:{checkIn.RoomNo}");
                    return BadRequest($"There is no Booking Available on the RoomNo:{checkIn.RoomNo}");
                }

                return Ok($"{checkIn.RoomNo} is Checked In");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<Booking>> SearchForBooking([FromQuery] BookingSearchDTO bookingSearch)
        {
            try
            {
                _logger.LogInformation("BookingController.SearchForBooking Method Called");

                List<BookingDTO> booking = new List<BookingDTO>();

                //Search for booking by bookingDate or guestPhoneNo or guestName
                _logger.LogInformation($"_bookingService.SearchBooking Method called with parameters bookingDate: {bookingSearch.BookingDate}, guestPhoneNo: {bookingSearch.GuestPhoneNo} and guestName: {bookingSearch.GuestName}");
                booking = _bookingService.SearchBooking(bookingSearch.BookingDate, bookingSearch.GuestPhoneNo, bookingSearch.GuestName);
                //Checking booking is existing or not
                if (booking.Count != 0)
                {
                    //Returning the Booking Details
                    _logger.LogInformation($"Booking details returned");
                    return Ok(booking);
                }
                else
                {
                    //Returned Empty result
                    _logger.LogInformation("Returned Empty result");
                    return NoContent();
                }
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
