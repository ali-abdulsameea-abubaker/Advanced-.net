using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is OrganizationsController that create api for each post , get , put, and delete.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly HealthDataContext _context;

        public OrganizationsController(HealthDataContext context)
        {
            _context = context;
        }
        // GET: api/Organizations
        [HttpGet]
        public async Task<ActionResult<OrganizationList>> GetOrganizations()
        {
            // Fetch the list of organizations from the database
            var organizations = await _context.Organization.ToListAsync();

            // Wrap the list in the OrganizationList class
            var organizationList = new OrganizationList
            {
                Organizations = organizations
            };

            // Return the wrapped list
            return Ok(organizationList);
        }

        // GET: api/Organizations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Organization>> GetOrganization(Guid id)
        {
            var organization = await _context.Organization.FindAsync(id);

            if (organization == null)
            {
                return NotFound(new { Message = "Organization not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            return Ok(organization); // Wrap the result in Ok()
        }
        // PUT: api/Organizations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrganization(Guid id, Organization organization)
        {
            if (id != organization.Id)
            {
                return BadRequest(new { Message = "ID mismatch.", StatusCode = 400, Id = Guid.NewGuid() });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            organization.UpdatedTime = DateTimeOffset.UtcNow;
            _context.Entry(organization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrganizationExists(id))
                {
                    return NotFound(new { Message = "Organization not found.", StatusCode = 404, Id = Guid.NewGuid() });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/Organizations
        [HttpPost]
        public async Task<ActionResult<Organization>> PostOrganization(Organization organization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            organization.Id = Guid.NewGuid();
            organization.UpdatedTime = DateTimeOffset.UtcNow;
            _context.Organization.Add(organization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrganization", new { id = organization.Id }, organization);
        }
        // DELETE: api/Organizations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganization(Guid id)
        {
            var organization = await _context.Organization.FindAsync(id);
            if (organization == null)
            {
                return NotFound(new { Message = "Organization not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            _context.Organization.Remove(organization);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool OrganizationExists(Guid id)
        {
            return _context.Organization.Any(e => e.Id == id);
        }
    }
}