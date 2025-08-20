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

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarAssignment> CarAssignments { get; set; }

    public virtual DbSet<CarAssignmentStatus> CarAssignmentStatuses { get; set; }

    public virtual DbSet<Cccd> Cccds { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassMember> ClassMembers { get; set; }

    public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }

    public virtual DbSet<ClassTime> ClassTimes { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<HealthCertificate> HealthCertificates { get; set; }

    public virtual DbSet<InstructorSpecialization> InstructorSpecializations { get; set; }

    public virtual DbSet<LearningApplication> LearningApplications { get; set; }

    public virtual DbSet<LearningMaterial> LearningMaterials { get; set; }

    public virtual DbSet<LicenceType> LicenceTypes { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ScheduleSlot> ScheduleSlots { get; set; }

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
            entity.HasKey(e => e.AddressId).HasName("PK__Addresse__CAA247C8B6EFE8EE");

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

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__20D6A968B8D70944");

            entity.ToTable("Attendance");

            entity.HasIndex(e => new { e.LearnerId, e.ClassId, e.SessionDate, e.ClassTimeId }, "UQ__Attendan__824E7FF756A9A498").IsUnique();

            entity.Property(e => e.AttendanceId).HasColumnName("attendance_id");
            entity.Property(e => e.AttendanceStatus).HasColumnName("attendance_status");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassTimeId).HasColumnName("class_time_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.PracticalDistance).HasColumnName("practical_distance");
            entity.Property(e => e.PracticalDurationHours).HasColumnName("practical_duration_hours");
            entity.Property(e => e.SessionDate).HasColumnName("session_date");

            entity.HasOne(d => d.Class).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__class__6383C8BA");

            entity.HasOne(d => d.ClassTime).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ClassTimeId)
                .HasConstraintName("FK__Attendanc__class_time");

            entity.HasOne(d => d.Learner).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.LearnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__learn__628FA481");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Cars__4C9A0DB31B0D4D40");

            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.CarMake)
                .HasMaxLength(50)
                .HasColumnName("car_make");
            entity.Property(e => e.CarModel)
                .HasMaxLength(50)
                .HasColumnName("car_model");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(13)
                .HasColumnName("license_plate");
        });

        modelBuilder.Entity<CarAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PK__CarAssig__DA8918145F5621F7");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.CarStatus).HasColumnName("car_status");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.ScheduleDate).HasColumnName("schedule_date");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.Car).WithMany(p => p.CarAssignments)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarAssign__car_i__00200768");

            entity.HasOne(d => d.CarStatusNavigation).WithMany(p => p.CarAssignments)
                .HasForeignKey(d => d.CarStatus)
                .HasConstraintName("FK__CarAssign__car_s__02FC7413");

            entity.HasOne(d => d.Instructor).WithMany(p => p.CarAssignments)
                .HasForeignKey(d => d.InstructorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarAssign__instr__01142BA1");

            entity.HasOne(d => d.Slot).WithMany(p => p.CarAssignments)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarAssign__slot___02084FDA");
        });

        modelBuilder.Entity<CarAssignmentStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__CarAssig__3683B53159251E2A");

            entity.ToTable("CarAssignmentStatus");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
        });

        modelBuilder.Entity<Cccd>(entity =>
        {
            entity.HasKey(e => e.CccdId).HasName("PK__CCCD__E13426B71085D601");

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
            entity.HasKey(e => e.CityId).HasName("PK__Cities__031491A8B5094E9C");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CityName)
                .HasMaxLength(100)
                .HasColumnName("city_name");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__FDF4798648621710");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(30)
                .HasColumnName("class_name");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");

            entity.HasOne(d => d.Course).WithMany(p => p.Classes)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Classes__course___5AEE82B9");

            entity.HasOne(d => d.Instructor).WithMany(p => p.Classes)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Classes__instruc__59FA5E80");
        });

        modelBuilder.Entity<ClassMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClassMem__3213E83FB10DBBCB");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassMembers)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__ClassMemb__class__5DCAEF64");

            entity.HasOne(d => d.Learner).WithMany(p => p.ClassMembers)
                .HasForeignKey(d => d.LearnerId)
                .HasConstraintName("FK__ClassMemb__learn__5EBF139D");
        });

        modelBuilder.Entity<ClassSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__ClassSch__C46A8A6F8BEBDDDB");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ScheduleDate).HasColumnName("schedule_date");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSchedules)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__ClassSche__class__787EE5A0");

            entity.HasOne(d => d.Slot).WithMany(p => p.ClassSchedules)
                .HasForeignKey(d => d.SlotId)
                .HasConstraintName("FK__ClassSche__slot___797309D9");
        });

        modelBuilder.Entity<ClassTime>(entity =>
        {
            entity.HasKey(e => e.ClassTimeId).HasName("PK__ClassTime");

            entity.ToTable("ClassTime");

            entity.HasIndex(e => new { e.ClassId, e.Thu, e.SlotId }, "UQ_ClassTime").IsUnique();

            entity.Property(e => e.ClassTimeId).HasColumnName("class_time_id");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.Thu).HasColumnName("thu");
            entity.Property(e => e.SlotId).HasColumnName("slot_id");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassTimes)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClassTime__class");

            entity.HasOne(d => d.Slot).WithMany(p => p.ClassTimes)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClassTime__slot");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__8F1EF7AED2872648");

            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.CourseName)
                .HasMaxLength(100)
                .HasColumnName("course_name");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.Courses)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__Courses__licence__571DF1D5");
        });

        modelBuilder.Entity<HealthCertificate>(entity =>
        {
            entity.HasKey(e => e.HealthCertificateId).HasName("PK__HealthCe__9EBB8D8527C973A5");

            entity.Property(e => e.HealthCertificateId).HasColumnName("health_certificate_id");
            entity.Property(e => e.ImageUrl)
                .IsUnicode(false)
                .HasColumnName("image_url");
        });

        modelBuilder.Entity<InstructorSpecialization>(entity =>
        {
            entity.HasKey(e => e.IsId).HasName("PK__Instruct__ADF81AD30D852928");

            entity.Property(e => e.IsId).HasColumnName("is_id");
            entity.Property(e => e.InstructorId).HasColumnName("instructor_id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");

            entity.HasOne(d => d.Instructor).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.InstructorId)
                .HasConstraintName("FK__Instructo__instr__66603565");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.InstructorSpecializations)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__Instructo__licen__6754599E");
        });

        modelBuilder.Entity<LearningApplication>(entity =>
        {
            entity.HasKey(e => e.LearningId).HasName("PK__Learning__C996F2D5233B71F8");

            entity.Property(e => e.LearningId).HasColumnName("learning_id");
            entity.Property(e => e.LearnerId).HasColumnName("learner_id");
            entity.Property(e => e.LearningStatus).HasColumnName("learning_status");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.ObstacleScore).HasColumnName("obstacle_score");
            entity.Property(e => e.PracticalDistance).HasColumnName("practical_distance");
            entity.Property(e => e.PracticalDurationHours).HasColumnName("practical_duration_hours");
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
            entity.HasKey(e => e.MaterialId).HasName("PK__Learning__6BFE1D2803961698");

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
                .HasConstraintName("FK__LearningM__licen__6E01572D");
        });

        modelBuilder.Entity<LicenceType>(entity =>
        {
            entity.HasKey(e => e.LicenceTypeId).HasName("PK__LicenceT__959FF8937890B072");

            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.LicenceCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("licence_code");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__4C27CCD875A70213");

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
                .HasConstraintName("FK__News__author_id__70DDC3D8");
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK__Province__08DCB60F1740F7F7");

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
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CC53D0F828");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<ScheduleSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK__Schedule__971A01BBB267F428");

            entity.Property(e => e.SlotId)
                .ValueGeneratedNever()
                .HasColumnName("slot_id");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
        });

        modelBuilder.Entity<TestApplication>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__TestAppl__F3FF1C0234D3847D");

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
            entity.Property(e => e.SimulationScore).HasColumnName("simulation_score");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SubmitProfileDate).HasColumnName("submit_profile_date");
            entity.Property(e => e.TheoryScore).HasColumnName("theory_score");

            entity.HasOne(d => d.Learning).WithMany(p => p.TestApplications)
                .HasForeignKey(d => d.LearningId)
                .HasConstraintName("FK__TestAppli__learn__6A30C649");
        });

        modelBuilder.Entity<TestScoreStandard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TestScor__3213E83F120143D1");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LicenceTypeId).HasColumnName("licence_type_id");
            entity.Property(e => e.MaxScore).HasColumnName("max_score");
            entity.Property(e => e.PartName)
                .HasMaxLength(50)
                .HasColumnName("part_name");
            entity.Property(e => e.PassScore).HasColumnName("pass_score");

            entity.HasOne(d => d.LicenceType).WithMany(p => p.TestScoreStandards)
                .HasForeignKey(d => d.LicenceTypeId)
                .HasConstraintName("FK__TestScore__licen__73BA3083");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FE09D8CD9");

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
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
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
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__B8D9ABA2970816A5");

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
            entity.HasKey(e => e.WardId).HasName("PK__Wards__396B899DB3E77CE3");

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
