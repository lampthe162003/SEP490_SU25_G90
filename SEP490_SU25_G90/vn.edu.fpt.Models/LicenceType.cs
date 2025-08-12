using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class LicenceType
{
    public byte LicenceTypeId { get; set; }

    public string LicenceCode { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<InstructorSpecialization> InstructorSpecializations { get; set; } = new List<InstructorSpecialization>();

    public virtual ICollection<LearningApplication> LearningApplications { get; set; } = new List<LearningApplication>();

    public virtual ICollection<LearningMaterial> LearningMaterials { get; set; } = new List<LearningMaterial>();

    public virtual ICollection<TestScoreStandard> TestScoreStandards { get; set; } = new List<TestScoreStandard>();
}
