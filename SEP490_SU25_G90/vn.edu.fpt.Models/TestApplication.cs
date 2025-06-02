using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class TestApplication
{
    public int TestId { get; set; }

    public int? LearnerId { get; set; }

    public DateOnly? ExamDate { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public double? Score { get; set; }

    public bool? Status { get; set; }

    public string? Notes { get; set; }

    public virtual LearningApplication? Learner { get; set; }
}
