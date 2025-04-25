using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace assignment4.Models;
/// <summary>
/// I, Ali Abubaker, 000857347, certify that this material is my original work. No other person's work has been used without due acknowledgement and I have not made my work available to anyone else.
/// </summary>
public partial class Order
{
    [Key]
    public int OrderId { get; set; }
    [Display(Name = "Customer Id")]
    public string? CustomerId { get; set; }
    [Display(Name = "EmployeeId")]
    public int? EmployeeId { get; set; }
    [Display(Name = "Ordered")]
    public DateTime? OrderDate { get; set; }

    [Display(Name = "Shipped")]
    public DateTime? ShippedDate { get; set; }

    [Display(Name = "Shipper")]
    public virtual Shipper? ShipViaNavigation { get; set; }

    [Display(Name = "Freight")]
    public decimal? Freight { get; set; }

    [Display(Name = "Ship Name")]
    public string? ShipName { get; set; }

    [Display(Name = "Region")]
    public string? ShipRegion { get; set; }

    [Display(Name = "Country")]
    public string? ShipCountry { get; set; }
    public DateTime? RequiredDate { get; set; }
    public int? ShipVia { get; set; }
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipPostalCode { get; set; }
    public virtual Employee? Employee { get; set; }
    
}
