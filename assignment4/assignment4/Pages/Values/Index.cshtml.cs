using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using assignment4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace assignment4.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly NorthwindContext _context;

        public IndexModel(NorthwindContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;
        public SelectList EmployeeList { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? SelectedEmployeeId { get; set; }

        public async Task OnGetAsync()
        {
            var employees = await _context.Employees
                .OrderBy(e => e.LastName)
                .Select(e => new { e.EmployeeId, Name = e.FirstName + " "+  e.LastName })
                .ToListAsync();

            EmployeeList = new SelectList(employees, "EmployeeId", "Name");

            var query = _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.ShipViaNavigation)
                .Where(o => o.Freight >= 250);

            if (SelectedEmployeeId.HasValue)
            {
                query = query.Where(o => o.EmployeeId == SelectedEmployeeId.Value);
            }

            Order = await query.ToListAsync();
        }
    }
}
