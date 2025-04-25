using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
/// <summary>
///  <Authorship>I, Ali Abdulsameea, 000857347 certify that this material is my original work.
/// No other person's work has been used without due acknowledgement.</Authorship>
/// This is Organization to set up thr properties.
/// <author>Ali Abubaker - 000857347</author>
/// </summary>
namespace WebApplication1.model
{
    public class Organization
    {
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset UpdatedTime { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Name cannot exceed 256 characters.")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("Hospital|Clinic|Pharmacy", ErrorMessage = "Type must be one of: Hospital, Clinic, or Pharmacy.")]
        public string Type { get; set; }

        [Required]
        public string Address { get; set; }
    }
}