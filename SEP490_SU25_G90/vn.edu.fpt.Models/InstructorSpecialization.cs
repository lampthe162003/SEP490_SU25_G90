using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class InstructorSpecialization
{
    public int IsId { get; set; }

    public int InstructorId { get; set; }

    public byte LicenceTypeId { get; set; }

    public virtual User Instructor { get; set; } = null!;

    public virtual LicenceType LicenceType { get; set; } = null!;
}
