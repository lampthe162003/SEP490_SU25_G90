using System;
using System.Collections.Generic;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? ProfileImageUrl { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? Dob { get; set; }

    public bool? Gender { get; set; }

    public int? CccdId { get; set; }

    public int? HealthCertificateId { get; set; }

    public string? Phone { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Cccd? Cccd { get; set; }

    public virtual ICollection<ClassMember> ClassMembers { get; set; } = new List<ClassMember>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual HealthCertificate? HealthCertificate { get; set; }

    public virtual ICollection<InstructorSpecialization> InstructorSpecializations { get; set; } = new List<InstructorSpecialization>();

    public virtual ICollection<LearningApplication> LearningApplications { get; set; } = new List<LearningApplication>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<TestApplication> TestApplications { get; set; } = new List<TestApplication>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
