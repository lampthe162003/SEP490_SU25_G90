using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class LearningApplication
{
    public int LearningId { get; set; }

    public int? LearnerId { get; set; }

    public byte? LicenceTypeId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public byte? LearningStatus { get; set; }

    public bool? PaymentStatus { get; set; }

    public int? InstructorId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public string? Notes { get; set; }

    public virtual User? Instructor { get; set; }

    public virtual User? Learner { get; set; }

    public virtual LicenceType? LicenceType { get; set; }

    public virtual ICollection<TestApplication> TestApplications { get; set; } = new List<TestApplication>();
}
