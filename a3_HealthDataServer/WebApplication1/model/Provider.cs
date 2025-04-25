using System;
using System.ComponentModel.DataAnnotations;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is Provider to set up thr properties.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.model
{
    public class Provider
    {
        public Guid Id { get; set; }
        [Required]
        public DateTimeOffset CreationTime { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "First Name cannot exceed 128 characters.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Last Name cannot exceed 128 characters.")]
        public string LastName { get; set; }
        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "License Number must be a positive number.")]
        public long LicenseNumber { get; set; }
        [Required]
        public string Address { get; set; }
    }
}