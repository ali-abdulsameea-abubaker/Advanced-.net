using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace assignment4.Models;
/// <summary>
/// I, Ali Abubaker, 000857347, certify that this material is my original work. No other person's work has been used without due acknowledgement and I have not made my work available to anyone else.
/// </summary>
public partial class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Title")]
    public string? Title { get; set; }

    [Display(Name = "Title of Courtesy")]
    public string? TitleOfCourtesy { get; set; }

    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }

    [Display(Name = "Hire Date")]
    [DataType(DataType.Date)]
    public DateTime? HireDate { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "City")]
    public string? City { get; set; }

    [Display(Name = "Region")]
    public string? Region { get; set; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [Display(Name = "Country")]
    public string? Country { get; set; }

    [Display(Name = "Home Phone")]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    public string? HomePhone { get; set; }

    [Display(Name = "Extension")]
    public string? Extension { get; set; }

    [Display(Name = "Notes")]
    [DataType(DataType.MultilineText)]
    public string? Notes { get; set; }

    [Display(Name = "Reports To")]
    public int? ReportsTo { get; set; }

    [Display(Name = "Photo")]
    public string? PhotoPath { get; set; }

    // This property won't be mapped to the database
    [NotMapped]
    [Display(Name = "Photo")]
    public IFormFile? PhotoUpload { get; set; }

    // Computed property for full name
    [NotMapped]
    [Display(Name = "Name")]
    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Employee? ReportsToNavigation { get; set; }
}