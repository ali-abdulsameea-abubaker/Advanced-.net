using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorIntro.Models;
using System.Security.Cryptography.X509Certificates;

namespace RazorIntro.Pages.Forms
{
    public class AddCourseModel : PageModel
    {
        [BindProperty]
        public Course? course { get; set; }
        public List<Course> addMoreCourse{ get; set; }
        public void OnGet()
        {
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
           
            return RedirectToPage("/index", new { course.CourseName});
        }

    }
}
