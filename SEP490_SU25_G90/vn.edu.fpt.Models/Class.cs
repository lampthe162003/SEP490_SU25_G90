﻿using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public int? InstructorId { get; set; }

    public byte? LicenceTypeId { get; set; }

    public string? ClassName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<ClassMember> ClassMembers { get; set; } = new List<ClassMember>();

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual User? Instructor { get; set; }

    public virtual LicenceType? LicenceType { get; set; }
}
