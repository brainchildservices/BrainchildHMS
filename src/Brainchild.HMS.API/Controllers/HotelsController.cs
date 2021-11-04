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

        public HotelsController(BrainchildHMSDbContext context,ILogger<HotelsController> logger,IConfiguration configuration)

        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _hotelService = new HotelService(_configuration.GetConnectionString("DefaultConnection"));
        }


        [HttpGet("{hotelId}/housekeeping")]
        public async Task<ActionResult<Hotel>> GetHouseKeeping(int hotelId)
        {
            try
            {
                _logger.LogInformation("HotelsController.GetHouseKeeping Method called");
                List<HouseKeepingDTO> houseKeepingDetails = new List<HouseKeepingDTO>();
                //Fetch the Rooms Details
                _logger.LogInformation($"_hotelService.GetHouseKeepingDetailsByHotelId Method called with parameter hotelID: {hotelId}");
                houseKeepingDetails= _hotelService.GetHouseKeepingDetailsByHotelId(hotelId);
                _logger.LogInformation("Fetched the Rooms Details");
                
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

                return Ok($"Updated the RoomNo: {houseKeeping.RoomNo} Status to {houseKeeping.RoomStatus}");

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
