using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTrackerRazor.Data;
using MovieTrackerRazor.Models;

namespace MovieTrackerRazor.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly MovieTrackerRazor.Data.MovieTrackerRazorContext _context;

        public IndexModel(MovieTrackerRazor.Data.MovieTrackerRazorContext context)
        {
            _context = context;

            if (!_context.Genre.Any())
            {
                SeedData.Initialize(_context);
            }
        }

        public IList<Movie> Movie { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string  SearchString { get; set; }
        public async Task OnGetAsync()
        {
            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(m => m.Title.Contains(SearchString));
            }
            Movie = await movies
              .Include(m => m.Genre)
              .AsNoTracking()
              .ToListAsync();

        }



    }
}
