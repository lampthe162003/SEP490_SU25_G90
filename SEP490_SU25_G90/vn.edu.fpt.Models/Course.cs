using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public byte? LicenceTypeId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual LicenceType? LicenceType { get; set; }
}
