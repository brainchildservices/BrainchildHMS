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
namespace Brainchild.HMS.Web.Controllers
{
    [Route("hms/api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Api"), IgnoreAntiforgeryToken, AllowAnonymous]
    public class ChargeTypesController : ControllerBase
    {
        private readonly BrainchildHMSDbContext _context;
        private readonly ILogger<ChargeTypesController> _logger;

        public ChargeTypesController(BrainchildHMSDbContext context,ILogger<ChargeTypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/ChargeTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChargeType>>> GetChargeTypes()
        {
            return await _context.ChargeTypes.ToListAsync();
        }

        // GET: api/ChargeTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChargeType>> GetChargeType(int id)
        {
            var chargeType = await _context.ChargeTypes.FindAsync(id);

            if (chargeType == null)
            {
                return NotFound();
            }

            return chargeType;
        }

        // PUT: api/ChargeTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChargeType(int id, ChargeType chargeType)
        {
            if (id != chargeType.ChargeTypeId)
            {
                return BadRequest();
            }

            _context.Entry(chargeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChargeTypeExists(id))
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

        // POST: api/ChargeTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChargeType>> PostChargeType(ChargeType chargeType)
        {
            _context.ChargeTypes.Add(chargeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChargeType", new { id = chargeType.ChargeTypeId }, chargeType);
        }

        // DELETE: api/ChargeTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChargeType(int id)
        {
            var chargeType = await _context.ChargeTypes.FindAsync(id);
            if (chargeType == null)
            {
                return NotFound();
            }

            _context.ChargeTypes.Remove(chargeType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChargeTypeExists(int id)
        {
            return _context.ChargeTypes.Any(e => e.ChargeTypeId == id);
        }
    }
}
