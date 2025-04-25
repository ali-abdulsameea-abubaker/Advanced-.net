using System;
using System.Collections.Generic;

namespace assignment4.Models;
/// <summary>
/// I, Ali Abubaker, 000857347, certify that this material is my original work. No other person's work has been used without due acknowledgement and I have not made my work available to anyone else.
/// </summary>
public partial class Shipper
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
