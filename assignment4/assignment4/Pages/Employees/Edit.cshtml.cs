using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment4.Models;

namespace assignment4.Pages.Employees;

public class EditModel : PageModel
{
    private readonly NorthwindContext _context;
    private readonly IWebHostEnvironment _env;

    public EditModel(NorthwindContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [BindProperty]
    public Employee Employee { get; set; }

    [BindProperty]
    public IFormFile PhotoUpload { get; set; }

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingEmployee = await _context.Employees.FindAsync(Employee.EmployeeId);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        if (PhotoUpload != null && PhotoUpload.Length > 0)
        {
            try
            {
                if (!string.IsNullOrEmpty(existingEmployee.PhotoPath))
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, existingEmployee.PhotoPath.TrimStart('~', '/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                var fileExtension = Path.GetExtension(PhotoUpload.FileName).ToLower();
                var safeFileName = $"employee_{Employee.EmployeeId}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
                var uploadsFolder = Path.Combine(_env.WebRootPath, "Images");
                var filePath = Path.Combine(uploadsFolder, safeFileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoUpload.CopyToAsync(stream);
                }

                Employee.PhotoPath = $"~/Images/{safeFileName}";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PhotoUpload", $"Error saving photo: {ex.Message}");
                return Page();
            }
        }
        else
        {
            Employee.PhotoPath = existingEmployee.PhotoPath;
        }

        _context.Entry(existingEmployee).CurrentValues.SetValues(Employee);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(Employee.EmployeeId))
            {
                return NotFound();
            }
            else
            {
                // Re-throw the exception so the compiler knows this path exits too
                throw;
            }
        }

        return Page();
    }


    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.EmployeeId == id);
    }
}
