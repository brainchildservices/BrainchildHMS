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


using Microsoft.Extensions.Configuration;
using Brainchild.HMS.Data;
using Brainchild.HMS.Data.DTOs;
using Newtonsoft.Json;

namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken]
    public class HotelsController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<HotelsController> _logger;
        private static IConfiguration _configuration;
        public IHotelService _hotelService;

        public HotelsController(BrainchildHMSDbContext context, ILogger<HotelsController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _hotelService = new HotelService(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("{hotelId}/checkout")]
        public async Task<ActionResult<Hotel>> GetCheckoutDetails(int hotelId, [FromQuery] CheckOutDTO checkOut)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetCheckkoutDetails Method called");
                List<Charge> charges = new List<Charge>();
                //Fetching Charge details
                _logger.LogInformation($"_hotelService.GetCharges Method called with parameters BookingId: {checkOut.BookingId} and RoomId: {checkOut.RoomId}");
                charges = _hotelService.GetCharges(checkOut.BookingId, checkOut.RoomId);
                CheckoutDetailsDTO checkoutDetails = new CheckoutDetailsDTO();
                //Fetching chekcout details
                _logger.LogInformation($"_hotelService.GetCheckoutDetails Method called with BookingId: {checkOut.BookingId} and RoomId: {checkOut.RoomId} and HotelId: {hotelId}");
                checkoutDetails = _hotelService.GetCheckoutDetails(checkOut.BookingId, checkOut.RoomId, hotelId);
                if (checkoutDetails.BookingId != 0)
                {
                    checkoutDetails.charges = charges;
                    string jsonValue = JsonConvert.SerializeObject(checkoutDetails);
                    _logger.LogInformation($"Returned available Rooms Lists: {jsonValue}");
                    return Ok(checkoutDetails);
                }
                else
                {
                    return BadRequest($"{false}: No data found!");
                }
                
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{hotelId}/housekeeping")]
        public async Task<ActionResult<Hotel>> GetHouseKeeping(int hotelId)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetHouseKeeping Method called");
                List<Room> houseKeepingDetails = new List<Room>();
                //Fetch the Rooms Details
                _logger.LogInformation($"_hotelService.GetHouseKeepingDetailsByHotelId Method called with parameter hotelID: {hotelId}");
                houseKeepingDetails = _hotelService.GetHouseKeepingDetailsByHotelId(hotelId);
                _logger.LogInformation("Fetched the Rooms Details");

                string jsonValue = JsonConvert.SerializeObject(houseKeepingDetails);
                _logger.LogInformation($"Returned available Rooms Lists: {jsonValue}");

                return Ok(houseKeepingDetails);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{hotelId}/housekeeping")]
        public async Task<ActionResult<Hotel>> HouseKeeping(int hotelId, HouseKeepingDTO houseKeeping)
        {
            try
            {
                _logger.LogInformation("HotelsController.HouseKeeping Method Called");

                //Changing the roomstatus
                _logger.LogInformation($"_hotelService.ChangeRoomStatus Method called with parameters {hotelId}, {houseKeeping.RoomNo}, {houseKeeping.RoomStatus}");
                _hotelService.ChangeRoomStatus(hotelId, houseKeeping.RoomNo, houseKeeping.RoomStatus);
                _logger.LogInformation($"Changed the status of the Room(RoomNo: {houseKeeping.RoomNo})");

                List<Room> houseKeepingDetails = new List<Room>();
                //Fetch the Rooms Details
                _logger.LogInformation($"_hotelService.GetHouseKeepingDetailsByHotelId Method called with parameter hotelID: {hotelId}");
                houseKeepingDetails = _hotelService.GetHouseKeepingDetailsByHotelId(hotelId);
                _logger.LogInformation("Fetched the Rooms Details");

                string jsonValue = JsonConvert.SerializeObject(houseKeepingDetails);
                _logger.LogInformation($"Updated the RoomNo: {houseKeeping.RoomNo} Status to {houseKeeping.RoomStatus}\n {jsonValue}");
                return Ok($"Updated the RoomNo: {houseKeeping.RoomNo} Status to {houseKeeping.RoomStatus}\n {jsonValue}");

            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{hotelId}/rooms")]
        public async Task<ActionResult<Hotel>> GetAvailableRooms(int hotelId, [FromQuery] AvailableRoomDTO availableRoom)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetAvailableRooms Method called");
                //creatng availableRoomList object for Room
                List<Room> availableRoomList = new List<Room>();
                //Selecting the available rooms
                _logger.LogInformation($"_hotelService.GetAvailableRoomList Method called with parameters {hotelId},{availableRoom.CheckInDate},{availableRoom.CheckOutDate},{availableRoom.RoomType} and {availableRoom.RoomStatus}");
                availableRoomList = _hotelService.GetAvailableRoomList(hotelId, availableRoom.CheckInDate, availableRoom.CheckOutDate, availableRoom.RoomType, availableRoom.RoomStatus);
                _logger.LogInformation("_hotelService.GetAvailableRoomList Method returned the available Rooms List");

                string jsonValue = JsonConvert.SerializeObject(availableRoomList);
                _logger.LogInformation($"Returned available Rooms Lists: {jsonValue}");

                //Returning the available roomlist
                return Ok(availableRoomList);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{hotelId}/roomplan")]
        public async Task<ActionResult<Hotel>> GetRoomPlan(int hotelId, RoomPlanDTO roomPlan)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetRoomPlan Method Called");
                List<RoomPlanDTO> roomPlanList = new List<RoomPlanDTO>();

                //Fetching the room plan details
                _logger.LogInformation($" _hotelService.GetRoomPlan Method called with the parameter fromDate:{roomPlan.FromDate},{roomPlan.ToDate} and hotelId: {hotelId}");
                roomPlanList = _hotelService.GetRoomPlan(roomPlan.FromDate, roomPlan.ToDate, hotelId);
                _logger.LogInformation("Fetched Room Plan Details");
                return Ok(roomPlanList);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{hotelId}/roomstats")]
        public async Task<ActionResult<Hotel>> GetRoomStats(int hotelId, RoomPlanDTO roomPlan)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetRoomStats Method Called");
                //Creating an object for RoomPlanSTO
                List<RoomPlanDTO> roomPlanList = new List<RoomPlanDTO>();
                //Fetching the rooms details
                _logger.LogInformation($" _hotelService.GetRoomPlan Method called with the parameter fromDate:{roomPlan.FromDate},{roomPlan.ToDate} and hotelId: {hotelId}");
                roomPlanList = _hotelService.GetRoomPlan(roomPlan.FromDate, roomPlan.ToDate, hotelId);
                _logger.LogInformation("Fetched Room Plan Details");


                return Ok(roomPlanList);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }
        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            _logger.LogInformation("Hello From HotelsController");
            return await _context.Hotels.ToListAsync();
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return hotel;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, Hotel hotel)
        {
            if (id != hotel.HotelID)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]

        public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.HotelID }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HotelExists(int id)
        {
            return _context.Hotels.Any(e => e.HotelID == id);
        }
    }
}
