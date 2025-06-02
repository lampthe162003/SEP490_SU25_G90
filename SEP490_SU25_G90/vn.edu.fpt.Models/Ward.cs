using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Ward
{
    public int WardId { get; set; }

    public string? WardName { get; set; }

    public int? ProvinceId { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Province? Province { get; set; }
}
