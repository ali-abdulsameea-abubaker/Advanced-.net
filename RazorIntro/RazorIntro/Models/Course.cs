using System.ComponentModel.DataAnnotations;

namespace RazorIntro.Models
{
    public class Course
    {
        [Display(Name = "Course Code")]
        public string? CourseCode { get; set; }
        [Display(Name = "Course Name")]
        public string? CourseName { get;
            set; }

        public int? Hours { get; set; }
    }
}
