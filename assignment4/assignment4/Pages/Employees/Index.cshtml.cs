using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Models;

namespace assignment4.Pages.Employees
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext _context;

        public IndexModel(NorthwindContext context)
        {
            _context = context;
        }

        public IList<Employee> Employees { get; set; }

        public async Task OnGetAsync()
        {
            Employees = await _context.Employees
                .Include(e => e.ReportsToNavigation)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}