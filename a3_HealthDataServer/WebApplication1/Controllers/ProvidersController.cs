using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is ProvidersController hat create api for each post , get , put, and delete.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly HealthDataContext _context;
        public ProvidersController(HealthDataContext context)
        {
            _context = context;
        }
        // GET: api/Providers
        [HttpGet]
        public async Task<ActionResult<ProviderList>> GetProviders()
        {
            // Fetch the list of providers from the database
            var providers = await _context.Provider.ToListAsync();

            // Wrap the list in the ProviderList class
            var providerList = new ProviderList
            {
                Providers = providers
            };

            // Return the wrapped list
            return Ok(providerList);
        }
        // GET: api/Providers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Provider>> GetProvider(Guid id)
        {
            var provider = await _context.Provider.FindAsync(id);

            if (provider == null)
            {
                return NotFound(new { Message = "Provider not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            return Ok(provider); // Wrap the result in Ok()
        }
        // PUT: api/Providers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvider(Guid id, Provider provider)
        {
            if (id != provider.Id)
            {
                return BadRequest(new { Message = "ID mismatch.", StatusCode = 400, Id = Guid.NewGuid() });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(provider).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProviderExists(id))
                {
                    return NotFound(new { Message = "Provider not found.", StatusCode = 404, Id = Guid.NewGuid() });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Providers
        [HttpPost]
        public async Task<ActionResult<Provider>> PostProvider(Provider provider)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            provider.Id = Guid.NewGuid();
            provider.CreationTime = DateTimeOffset.UtcNow;
            _context.Provider.Add(provider);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProvider", new { id = provider.Id }, provider);
        }

        // DELETE: api/Providers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvider(Guid id)
        {
            var provider = await _context.Provider.FindAsync(id);
            if (provider == null)
            {
                return NotFound(new { Message = "Provider not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            _context.Provider.Remove(provider);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ProviderExists(Guid id)
        {
            return _context.Provider.Any(e => e.Id == id);
        }
    }
}