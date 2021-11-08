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

namespace Brainchild.HMS.API.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class ChargesController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<ChargesController> _logger;
        private static IConfiguration _configuration;
        public IChargeService _chargeService;
        public ChargesController(BrainchildHMSDbContext context, ILogger<ChargesController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _chargeService = new ChargeService(_configuration.GetConnectionString("DefaultConnection"));
        }
       
        // GET: api/Charges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Charge>>> GetCharges()
        {
            return await _context.Charges.ToListAsync();
        }

        // GET: api/Charges/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Charge>> GetCharge(int id)
        {
            var charge = await _context.Charges.FindAsync(id);

            if (charge == null)
            {
                return NotFound();
            }

            return charge;
        }

        // PUT: api/Charges/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharge(int id, Charge charge)
        {
            if (id != charge.ChargeId)
            {
                return BadRequest();
            }

            _context.Entry(charge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChargeExists(id))
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

        // POST: api/Charges
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Charge>> PostCharge(ChargeDTO charge)
        {
            try
            {
                //Creating charges
                _logger.LogInformation($" _chargeService.AddCharges() Method called with parameters {charge.ChargeTypeId}, {charge.CurrencyId}, {charge.ChargeAmount}, {charge.BookingId} and {charge.RoomId}");
                _chargeService.AddCharges(charge);
                _logger.LogInformation("Charges Added Successfully.");

                return CreatedAtAction("GetCharge", new { id = charge.ChargeId }, charge);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Exception: {exception}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Charges/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharge(int id)
        {
            var charge = await _context.Charges.FindAsync(id);
            if (charge == null)
            {
                return NotFound();
            }

            _context.Charges.Remove(charge);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChargeExists(int id)
        {
            return _context.Charges.Any(e => e.ChargeId == id);
        }
    }
}
