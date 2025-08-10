using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int LearnerId { get; set; }

    public int ClassId { get; set; }

    public DateOnly SessionDate { get; set; }

    public bool? AttendanceStatus { get; set; }

    public double? PracticalDurationHours { get; set; }

    public double? PracticalDistance { get; set; }

    public string? Note { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual LearningApplication Learner { get; set; } = null!;
}
