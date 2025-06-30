using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class TestApplication
{
    public int TestId { get; set; }

    public int? LearningId { get; set; }

    public DateOnly? ExamDate { get; set; }

    public string? ResultImageUrl { get; set; }

    public int? TheoryScore { get; set; }

    public int? SimulationScore { get; set; }

    public int? ObstacleScore { get; set; }

    public int? PracticalScore { get; set; }

    public bool? Status { get; set; }

    public DateOnly? SubmitProfileDate { get; set; }

    public string? Notes { get; set; }

    public virtual LearningApplication? Learning { get; set; }
}
