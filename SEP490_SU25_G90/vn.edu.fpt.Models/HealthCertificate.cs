using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class HealthCertificate
{
    public int HealthCertificateId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
