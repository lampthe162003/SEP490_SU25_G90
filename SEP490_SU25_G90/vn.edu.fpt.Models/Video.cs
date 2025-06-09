using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Video
{
    public int VideoId { get; set; }

    public string? VideoLink { get; set; }

    public int? CourseId { get; set; }

    public bool? ActiveStatus { get; set; }

    public virtual Course? Course { get; set; }
}
