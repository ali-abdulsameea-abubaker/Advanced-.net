using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Models;

namespace assignment4.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly NorthwindContext _context;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(NorthwindContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (Employee == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<int> HasSubordinates()
        {
            return await _context.Employees.CountAsync(e => e.ReportsTo == Employee.EmployeeId);
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = await _context.Employees.FindAsync(id);

            if (Employee != null)
            {
                // Handle subordinates by setting their ReportsTo to null
                var subordinates = await _context.Employees
                    .Where(e => e.ReportsTo == id)
                    .ToListAsync();

                foreach (var subordinate in subordinates)
                {
                    subordinate.ReportsTo = null;
                }

                // Delete photo if exists
                if (!string.IsNullOrEmpty(Employee.PhotoPath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, Employee.PhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Employees.Remove(Employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee deleted successfully!";
            }

            return RedirectToPage("./Index");
        }
    }
}