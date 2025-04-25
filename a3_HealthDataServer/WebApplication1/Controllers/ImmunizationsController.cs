using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is ImmunizationsController that create api for each post , get , put, and delete.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImmunizationsController : ControllerBase
    {
        private readonly HealthDataContext _context;

        public ImmunizationsController(HealthDataContext context)
        {
            _context = context;
        }
        // GET: api/Immunizations
        [HttpGet]
        public async Task<ActionResult<ImmunizationList>> GetImmunizations()
        {
            // Fetch the list of immunizations from the database
            var immunizations = await _context.Immunization.ToListAsync();

            // Wrap the list in the ImmunizationList class
            var immunizationList = new ImmunizationList
            {
                Immunizations = immunizations
            };

            // Return the wrapped list
            return Ok(immunizationList);
        }
        // GET: api/Immunizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Immunization>> GetImmunization(Guid id)
        {
            var immunization = await _context.Immunization.FindAsync(id);

            if (immunization == null)
            {
                return NotFound(new { Message = "Immunization not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            return Ok(immunization); // Wrap the result in Ok()
        }
        // PUT: api/Immunizations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImmunization(Guid id, Immunization immunization)
        {
            if (id != immunization.Id)
            {
                return BadRequest(new { Message = "ID mismatch.", StatusCode = 400, Id = Guid.NewGuid() });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            immunization.UpdatedTime = DateTimeOffset.UtcNow;
            _context.Entry(immunization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImmunizationExists(id))
                {
                    return NotFound(new { Message = "Immunization not found.", StatusCode = 404, Id = Guid.NewGuid() });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/Immunizations
        [HttpPost]
        public async Task<ActionResult<Immunization>> PostImmunization(Immunization immunization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            immunization.Id = Guid.NewGuid();
            immunization.CreationTime = DateTimeOffset.UtcNow;
            _context.Immunization.Add(immunization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImmunization", new { id = immunization.Id }, immunization);
        }

        // DELETE: api/Immunizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImmunization(Guid id)
        {
            var immunization = await _context.Immunization.FindAsync(id);
            if (immunization == null)
            {
                return NotFound(new { Message = "Immunization not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            _context.Immunization.Remove(immunization);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImmunizationExists(Guid id)
        {
            return _context.Immunization.Any(e => e.Id == id);
        }
    }
}