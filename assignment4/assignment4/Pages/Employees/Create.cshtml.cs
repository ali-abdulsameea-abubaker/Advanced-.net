using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using assignment4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace assignment4.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly NorthwindContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(NorthwindContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Employee Employee { get; set; }

        [BindProperty]
        public IFormFile PhotoUpload { get; set; }

        public SelectList Managers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadManagersAsync();
            return Page();
        }

       

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadManagersAsync();
                return Page();
            }

            if (PhotoUpload != null && PhotoUpload.Length > 0)
            {
                try
                {
                    // Get original filename and extension
                    var originalFileName = Path.GetFileNameWithoutExtension(PhotoUpload.FileName);
                    var fileExtension = Path.GetExtension(PhotoUpload.FileName);

                    // Create a safe filename (remove special characters)
                    var safeFileName = $"{originalFileName}_{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";
                    safeFileName = string.Join("_", safeFileName.Split(Path.GetInvalidFileNameChars()));

                    var uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
                    var filePath = Path.Combine(uploadsFolder, safeFileName);

                    // Ensure directory exists
                    Directory.CreateDirectory(uploadsFolder);

                    // Check if file exists and append number if needed
                    int counter = 1;
                    while (System.IO.File.Exists(filePath))
                    {
                        safeFileName = $"{originalFileName}_{Guid.NewGuid().ToString().Substring(0, 8)}_{counter}{fileExtension}";
                        filePath = Path.Combine(uploadsFolder, safeFileName);
                        counter++;
                    }

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoUpload.CopyToAsync(stream);
                    }

                    Employee.PhotoPath = $"/Images/{safeFileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PhotoUpload", $"Error saving photo: {ex.Message}");
                    await LoadManagersAsync();
                    return Page();
                }
            }

            _context.Employees.Add(Employee);
            await _context.SaveChangesAsync();

            // Add the success message here (not in the catch block)
            TempData["SuccessMessage"] = "Employee created successfully!";
            return Page();
        }

        private async Task LoadManagersAsync()
        {
            var managers = await _context.Employees
                .Where(e => Employee == null || e.EmployeeId != Employee.EmployeeId) // Handle null Employee safely
                .Select(e => new {
                    e.EmployeeId,
                    FullName = e.FirstName + " " + e.LastName
                })
                .ToListAsync();

            Managers = new SelectList(managers, "EmployeeId", "FullName");
        }
    }
}