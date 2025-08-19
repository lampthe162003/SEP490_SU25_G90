using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class CarAssignmentStatus
{
    public byte StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<CarAssignment> CarAssignments { get; set; } = new List<CarAssignment>();
}
