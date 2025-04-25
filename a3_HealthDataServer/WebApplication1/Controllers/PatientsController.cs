using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is PatientsController hat create api for each post , get , put, and delete.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly HealthDataContext _context;

        public PatientsController(HealthDataContext context)
        {
            _context = context;
        }
        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<PatientList>> GetPatients()
        {
            // Fetch the list of patients from the database
            var patients = await _context.Patient.ToListAsync();

            // Wrap the list in the PatientList class
            var patientList = new PatientList
            {
                Patients = patients
            };

            // Return the wrapped list
            return Ok(patientList);
        }
        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patient.FindAsync(id);

            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            return Ok(patient); // Wrap the result in Ok()
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(Guid id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest(new { Message = "ID mismatch.", StatusCode = 400, Id = Guid.NewGuid() });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound(new { Message = "Patient not found.", StatusCode = 404, Id = Guid.NewGuid() });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            patient.Id = Guid.NewGuid();
            patient.CreationTime = DateTimeOffset.UtcNow;
            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patient.FindAsync(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found.", StatusCode = 404, Id = Guid.NewGuid() });
            }

            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patient.Any(e => e.Id == id);
        }
    }
}