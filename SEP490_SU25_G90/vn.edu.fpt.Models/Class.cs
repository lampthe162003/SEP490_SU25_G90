using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public int? InstructorId { get; set; }

    public byte? LicenceTypeId { get; set; }

    public string? ClassName { get; set; }

    public virtual ICollection<ClassMember> ClassMembers { get; set; } = new List<ClassMember>();

    public virtual User? Instructor { get; set; }

    public virtual LicenceType? LicenceType { get; set; }
}
