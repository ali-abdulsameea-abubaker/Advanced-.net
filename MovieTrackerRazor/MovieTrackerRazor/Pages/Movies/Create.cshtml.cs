using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTrackerRazor.Data;
using MovieTrackerRazor.Models;

namespace MovieTrackerRazor.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly MovieTrackerRazor.Data.MovieTrackerRazorContext _context;

        public CreateModel(MovieTrackerRazor.Data.MovieTrackerRazorContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["GenreId"] = new SelectList(_context.Set<Genre>(), "GenreId", "GenreDescription");
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Movie == null || Movie == null)
            {
                return Page();
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
