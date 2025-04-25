using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorIntro.Models;

namespace RazorIntro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        [BindProperty(SupportsGet =true)]
        public string? CourseName { get; set; }
      

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            if (string.IsNullOrEmpty(CourseName))
            {
                CourseName = "Mohawk college";
            }

        }
    }
}
