using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class ClassMember
{
    public int Id { get; set; }

    public int? ClassId { get; set; }

    public int? LearnerId { get; set; }

    public virtual Class? Class { get; set; }

    public virtual LearningApplication? Learner { get; set; }
}
