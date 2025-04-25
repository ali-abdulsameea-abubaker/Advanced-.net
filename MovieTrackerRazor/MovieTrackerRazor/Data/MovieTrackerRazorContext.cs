using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTrackerRazor.Models;

namespace MovieTrackerRazor.Data
{
    public class MovieTrackerRazorContext : DbContext
    {
        public MovieTrackerRazorContext (DbContextOptions<MovieTrackerRazorContext> options)
            : base(options)
        {
        }

        public DbSet<MovieTrackerRazor.Models.Movie> Movie { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; }
    }
}
