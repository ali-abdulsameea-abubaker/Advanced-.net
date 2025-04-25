using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace assignment4.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly NorthwindContext _context;
        private readonly IWebHostEnvironment _env;

        public DetailsModel(NorthwindContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _context.Employees
                .Include(e => e.ReportsToNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }

        public string GetOriginalFileName()
        {
            if (string.IsNullOrEmpty(Employee.PhotoPath))
                return "No photo available";

            var fileName = Path.GetFileName(Employee.PhotoPath);
            // Extract original name before the GUID we added
            var parts = fileName.Split('_');
            if (parts.Length > 2)
            {
                return $"{parts[0]}{Path.GetExtension(fileName)}";
            }
            return fileName;
        }
    }
}