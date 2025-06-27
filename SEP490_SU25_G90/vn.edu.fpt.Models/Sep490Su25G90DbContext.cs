using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class Sep490Su25G90DbContext : DbContext
{
    public Sep490Su25G90DbContext()
    {
    }

    public Sep490Su25G90DbContext(DbContextOptions<Sep490Su25G90DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cccd> Cccds { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassMember> ClassMembers { get; set; }

    public virtual DbSet<HealthCertificate> HealthCertificates { get; set; }

    public virtual DbSet<InstructorSpecialization> InstructorSpecializations { get; set; }

    public virtual DbSet<LearningApplication> LearningApplications { get; set; }

    public virtual DbSet<LearningMaterial> LearningMaterials { get; set; }

    public virtual DbSet<LicenceType> LicenceTypes { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TestApplication> TestApplications { get; set; }

    public virtual DbSet<TestScoreStandard> TestScoreStandards { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__CAA247C822D7847B");

            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.HouseNumber)
                .HasMaxLength(100)
                .HasColumnName("house_number");
            entity.Property(e => e.RoadName)
                .HasMaxLength(100)
                .HasColumnName("road_name");
            entity.Property(e => e.WardId).HasColumnName("ward_id");

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK__Addresses__ward___46E78A0C");
        });

        modelBuilder.Entity<Cccd>(entity =>
        {
            entity.HasKey(e => e.CccdId).HasName("PK__CCCD__E13426B7AE73012C");

            entity.ToTable("CCCD");

            entity.Property(e => e.CccdId).HasColumnName("CCCD_id");
            entity.Property(e => e.CccdNumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCCD_number");
            entity.Property(e => e.ImageMs)
                .IsUnicode(false)
                .HasColumnName("image_ms");
            entity.Property(e => e.ImageMt)
                .IsUnicode(false)
                .HasColumnName("image_mt");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__031491A8107CAD2E");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__FDF47986E26BC571");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(30)
                .HasColumnName("class_name");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Classes)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Classes__instruc__571DF1D5");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.Classes)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__Classes__licence__5812160E");
        });

        modelBuilder.Entity<ClassMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClassMem__3213E83F1FDFDB23");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassMembers)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__ClassMemb__class__5AEE82B9");

            entity.HasOne(d => d.Learner).WithMany(p => p.ClassMembers)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__ClassMemb__learn__5BE2A6F2");
        });

        modelBuilder.Entity<HealthCertificate>(entity =>
        {
            entity.HasKey(e => e.HealthCertificateId).HasName("PK__HealthCe__9EBB8D854CFF4AA6");

            entity.Property(e => e.HealthCertificateId).HasColumnName("health_certificate_id");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<InstructorSpecialization>(entity =>
        {
            entity.HasKey(e => e.IsId).HasName("PK__Instruct__ADF81AD303B8759D");

            entity.Property(e => e.IsId).HasColumnName("is_id");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");

            entity.HasOne(d => d.Instructor).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Instructo__instr__5EBF139D");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__Instructo__licen__5FB337D6");
        });

        modelBuilder.Entity<LearningApplication>(entity =>
        {
            entity.HasKey(e => e.LearningId).HasName("PK__Learning__C996F2D57D4FBAED");

            entity.Property(e => e.LearningId).HasColumnName("learning_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.LearningStatus).HasColumnName("learning_status");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.ObstacleScore).HasColumnName("obstacle_score");
            entity.Property(e => e.PracticalScore).HasColumnName("practical_score");
            entity.Property(e => e.SimulationScore).HasColumnName("simulation_score");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");
            entity.Property(e => e.TestEligibility).HasColumnName("test_eligibility");
            entity.Property(e => e.TheoryScore).HasColumnName("theory_score");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearningApplications)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LearningA__learn__534D60F1");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.LearningApplications)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__LearningA__licen__5441852A");
        });

        modelBuilder.Entity<LearningMaterial>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Learning__6BFE1D2824897B31");

            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FileLink).HasColumnName("file_link");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.LearningMaterials)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__LearningM__licen__6754599E");
        });

        modelBuilder.Entity<LicenceType>(entity =>
        {
            entity.HasKey(e => e.LicenceTypeId).HasName("PK__LicenceT__959FF89320224CFC");

            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.LicenceCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("licence_code");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__4C27CCD8ABCEF2B6");

            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.NewsContent).HasColumnName("news_content");
            entity.Property(e => e.PostTime)
                .HasColumnType("datetime")
                .HasColumnName("post_time");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .HasColumnName("title");

            entity.HasOne(d => d.Author).WithMany(p => p.News)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK__News__author_id__6A30C649");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__08DCB60FBD99A5F0");

            entity.Property(e => e.ProvinceId)
                .ValueGeneratedNever()
                .HasColumnName("province_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.ProvinceName)
                .HasMaxLength(100)
                .HasColumnName("province_name");

            entity.HasOne(d => d.City).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Provinces__city___412EB0B6");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC8E4F2BC9");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TestApplication>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestAppl__F3FF1C0213D75CE8");

            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.ExamDate).HasColumnName("exam_date");
            entity.Property(e => e.LearningId).HasColumnName("learning_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ObstacleScore).HasColumnName("obstacle_score");
            entity.Property(e => e.PracticalScore).HasColumnName("practical_score");
            entity.Property(e => e.ResultImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("result_image_url");
            entity.Property(e => e.SignedAt)
                .HasColumnType("datetime")
                .HasColumnName("signed_at");
            entity.Property(e => e.SignedBy).HasColumnName("signed_by");
            entity.Property(e => e.SimulationScore).HasColumnName("simulation_score");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TheoryScore).HasColumnName("theory_score");

            entity.HasOne(d => d.Learning).WithMany(p => p.TestApplications)
                .HasForeignKey(d => d.LearningId)
                .HasConstraintName("FK__TestAppli__learn__628FA481");

            entity.HasOne(d => d.SignedByNavigation).WithMany(p => p.TestApplications)
                .HasForeignKey(d => d.SignedBy)
                .HasConstraintName("FK__TestAppli__signe__6383C8BA");
        });

        modelBuilder.Entity<TestScoreStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestScor__3213E83F63F03BB4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.MaxScore).HasColumnName("max_score");
            entity.Property(e => e.PartName)
                .HasMaxLength(50)
                .HasColumnName("part_name");
            entity.Property(e => e.PassScore).HasColumnName("pass_score");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.TestScoreStandards)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__TestScore__licen__6D0D32F4");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FB976D7DA");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CccdId).HasColumnName("CCCD_id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(10)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.HealthCertificateId).HasColumnName("health_certificate_id");
            entity.Property(e => e.LastName)
                .HasMaxLength(10)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(20)
                .HasColumnName("middle_name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("profile_image_url");

            entity.HasOne(d => d.Address).WithMany(p => p.Users)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Users__address_i__4BAC3F29");

            entity.HasOne(d => d.Cccd).WithMany(p => p.Users)
                .HasForeignKey(d => d.CccdId)
                .HasConstraintName("FK__Users__CCCD_id__49C3F6B7");

            entity.HasOne(d => d.HealthCertificate).WithMany(p => p.Users)
                .HasForeignKey(d => d.HealthCertificateId)
                .HasConstraintName("FK__Users__health_ce__4AB81AF0");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__B8D9ABA2218420EA");

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__role___4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__user___4E88ABD4");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.WardId).HasName("PK__Wards__396B899DB0074754");

            entity.Property(e => e.WardId)
                .ValueGeneratedNever()
                .HasColumnName("ward_id");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.WardName)
                .HasMaxLength(100)
                .HasColumnName("ward_name");

            entity.HasOne(d => d.Province).WithMany(p => p.Wards)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Wards__province___440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
