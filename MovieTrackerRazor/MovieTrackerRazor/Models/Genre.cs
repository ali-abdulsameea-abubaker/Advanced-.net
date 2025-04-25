using System.ComponentModel.DataAnnotations;

namespace MovieTrackerRazor.Models
{
    public class Genre
    {
        [Key] 
        public int GenreId { get; set; }
        [Required, StringLength(25)]
        public string GenreDescription { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
