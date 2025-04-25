using Microsoft.EntityFrameworkCore;
using WebApplication1.model;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is HealthDataContext that connectr to dtabase and get and  set up the properties.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.Data
{
    public class HealthDataContext : DbContext
    {
        public HealthDataContext (DbContextOptions<HealthDataContext> options)
            : base(options)
        {
           Database.EnsureCreated();
        }

        
        public DbSet<WebApplication1.model.Immunization> Immunization { get; set; } = default!;
        public DbSet<WebApplication1.model.Organization> Organization { get; set; } = default!;
        public DbSet<WebApplication1.model.Patient> Patient { get; set; } = default!;
        public DbSet<WebApplication1.model.Provider> Provider { get; set; } = default!;
    }
}
