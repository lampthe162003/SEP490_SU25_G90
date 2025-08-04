using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class LearningApplication
{
    public int LearningId { get; set; }

    public int LearnerId { get; set; }

    public byte? LicenceTypeId { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? TheoryScore { get; set; }

    public int? SimulationScore { get; set; }

    public int? ObstacleScore { get; set; }

    public int? PracticalScore { get; set; }

    public byte? LearningStatus { get; set; }

    public bool? TestEligibility { get; set; }

    public virtual ICollection<ClassMember> ClassMembers { get; set; } = new List<ClassMember>();

    public virtual User Learner { get; set; } = null!;

    public virtual LicenceType? LicenceType { get; set; }

    public virtual ICollection<TestApplication> TestApplications { get; set; } = new List<TestApplication>();
}
