using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Car
{
    public int CarId { get; set; }

    public string? LicensePlate { get; set; }

    public string? CarMake { get; set; }

    public string? CarModel { get; set; }

    public virtual ICollection<CarAssignment> CarAssignments { get; set; } = new List<CarAssignment>();
}
