using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class LearningMaterial
{
    public int MaterialId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? FileLink { get; set; }

    public DateTime? CreatedAt { get; set; }
}
