using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public byte? LicenceTypeId { get; set; }

    public bool? ActiveStatus { get; set; }

    public virtual LicenceType? LicenceType { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
