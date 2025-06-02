using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? HouseNumber { get; set; }

    public string? RoadName { get; set; }

    public int? WardId { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual Ward? Ward { get; set; }
}
