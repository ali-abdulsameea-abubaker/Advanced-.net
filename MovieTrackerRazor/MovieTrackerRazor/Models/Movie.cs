using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTrackerRazor.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Date Seen")]
        public DateTime? DateSeen { get; set; }
        [ForeignKey(nameof(Genre))]
        [Display(Name = "Genre")]
        public int? GenreId { get; set; }
        public Genre? Genre { get; set; }
        [Range(0, 10)]
        public int? Rating { get; set; }
        [StringLength(100), Display(Name ="Poster")]
        public string? ImageFile { get; set; }

    }
}
