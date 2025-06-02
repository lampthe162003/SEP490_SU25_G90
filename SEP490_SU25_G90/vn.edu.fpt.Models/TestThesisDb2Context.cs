using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SEP490_SU25_G90.vn.edu.fpt.Models;

public partial class TestThesisDb2Context : DbContext
{
    public TestThesisDb2Context()
    {
    }

    public TestThesisDb2Context(DbContextOptions<TestThesisDb2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cccd> Cccds { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<HealthCertificate> HealthCertificates { get; set; }

    public virtual DbSet<InstructorSpecialization> InstructorSpecializations { get; set; }

    public virtual DbSet<LearningApplication> LearningApplications { get; set; }

    public virtual DbSet<LearningMaterial> LearningMaterials { get; set; }

    public virtual DbSet<LicenceType> LicenceTypes { get; set; }

    public virtual DbSet<MockTestQuestion> MockTestQuestions { get; set; }

    public virtual DbSet<MockTestResult> MockTestResults { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TestApplication> TestApplications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=TestThesisDB2;user=Verliezer;pwd=123456;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__CAA247C8705A940D");

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
                .HasConstraintName("FK__Addresses__ward___44FF419A");
        });

        modelBuilder.Entity<Cccd>(entity =>
        {
            entity.HasKey(e => e.CccdId).HasName("PK__CCCD__E13426B76D7F0E2D");

            entity.ToTable("CCCD");

            entity.Property(e => e.CccdId).HasColumnName("CCCD_id");
            entity.Property(e => e.CccdNumber)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CCCD_number");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__031491A86021A390");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<HealthCertificate>(entity =>
        {
            entity.HasKey(e => e.HealthCertificateId).HasName("PK__HealthCe__9EBB8D85BBC6B158");

            entity.Property(e => e.HealthCertificateId).HasColumnName("health_certificate_id");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<InstructorSpecialization>(entity =>
        {
            entity.HasKey(e => e.IsId).HasName("PK__Instruct__ADF81AD3FE712193");

            entity.Property(e => e.IsId).HasColumnName("is_id");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");

            entity.HasOne(d => d.Instructor).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__instr__59FA5E80");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.LicenceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructo__licen__5AEE82B9");
        });

        modelBuilder.Entity<LearningApplication>(entity =>
        {
            entity.HasKey(e => e.LearningId).HasName("PK__Learning__C996F2D5AC664073");

            entity.Property(e => e.LearningId).HasColumnName("learning_id");
            entity.Property(e => e.AssignedAt)
                .HasColumnType("datetime")
                .HasColumnName("assigned_at");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.LearningStatus).HasColumnName("learning_status");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.PaymentStatus).HasColumnName("payment_status");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");

            entity.HasOne(d => d.Instructor).WithMany(p => p.LearningApplicationInstructors)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__LearningA__instr__571DF1D5");

            entity.HasOne(d => d.Learner).WithMany(p => p.LearningApplicationLearners)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__LearningA__learn__5535A963");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.LearningApplications)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__LearningA__licen__5629CD9C");
        });

        modelBuilder.Entity<LearningMaterial>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK__Learning__6BFE1D284D906242");

            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FileLink)
                .HasMaxLength(255)
                .HasColumnName("file_link");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<LicenceType>(entity =>
        {
            entity.HasKey(e => e.LicenceTypeId).HasName("PK__LicenceT__959FF8931A34ADCC");

            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.LicenceCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("licence_code");
        });

        modelBuilder.Entity<MockTestQuestion>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__MockTest__2EC21549893F329E");

            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.CorrectOption)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("correct_option");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.OptionA)
                .HasMaxLength(255)
                .HasColumnName("option_a");
            entity.Property(e => e.OptionB)
                .HasMaxLength(255)
                .HasColumnName("option_b");
            entity.Property(e => e.OptionC)
                .HasMaxLength(255)
                .HasColumnName("option_c");
            entity.Property(e => e.OptionD)
                .HasMaxLength(255)
                .HasColumnName("option_d");
            entity.Property(e => e.QuestionText).HasColumnName("question_text");
            entity.Property(e => e.ReadyStatus).HasColumnName("ready_status");
            entity.Property(e => e.TestType).HasColumnName("test_type");

            entity.HasOne(d => d.TestTypeNavigation).WithMany(p => p.MockTestQuestions)
                .HasForeignKey(d => d.TestType)
                .HasConstraintName("FK__MockTestQ__test___628FA481");
        });

        modelBuilder.Entity<MockTestResult>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__MockTest__AFB3C316B72D3771");

            entity.Property(e => e.ResultId).HasColumnName("result_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.MockTestResults)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__MockTestR__user___66603565");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__08DCB60FB9A64E56");

            entity.Property(e => e.ProvinceId)
                .ValueGeneratedNever()
                .HasColumnName("province_id");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.ProvinceName)
                .HasMaxLength(100)
                .HasColumnName("province_name");

            entity.HasOne(d => d.City).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__Provinces__city___3F466844");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC57853982");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<TestApplication>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestAppl__F3FF1C02B5D7ADC2");

            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.ExamDate).HasColumnName("exam_date");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SubmittedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");

            entity.HasOne(d => d.Learner).WithMany(p => p.TestApplications)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__TestAppli__learn__5EBF139D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FF580423E");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E6164EE57C498").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Users__F3DBC572A4C06825").IsUnique();

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
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

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
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__B8D9ABA261D0CDFF");

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
            entity.HasKey(e => e.WardId).HasName("PK__Wards__396B899DFD9420CD");

            entity.Property(e => e.WardId)
                .ValueGeneratedNever()
                .HasColumnName("ward_id");
            entity.Property(e => e.ProvinceId).HasColumnName("province_id");
            entity.Property(e => e.WardName)
                .HasMaxLength(100)
                .HasColumnName("ward_name");

            entity.HasOne(d => d.Province).WithMany(p => p.Wards)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK__Wards__province___4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
