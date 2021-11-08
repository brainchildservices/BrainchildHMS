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
using Brainchild.HMS.Data;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
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


        [HttpGet("{hotelId}/rooms")]
        public async Task<ActionResult<Hotel>> GetAvailableRooms(int hotelId,[FromQuery] AvailableRoomDTO availableRoom)
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
                roomPlanList = _hotelService.GetRoomPlan(roomPlan.FromDate,roomPlan.ToDate,hotelId);
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
