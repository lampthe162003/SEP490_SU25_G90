using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Cccd
{
    public int CccdId { get; set; }

    public string CccdNumber { get; set; } = null!;

    public string? ImageMt { get; set; }

    public string? ImageMs { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
