using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public int? InstructorId { get; set; }

    public string? ClassName { get; set; }

    public int? CourseId { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<ClassMember> ClassMembers { get; set; } = new List<ClassMember>();

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual ICollection<ClassTime> ClassTimes { get; set; } = new List<ClassTime>();

    public virtual Course? Course { get; set; }

    public virtual User? Instructor { get; set; }
}
