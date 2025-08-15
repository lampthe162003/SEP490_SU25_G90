using Microsoft.AspNetCore.Identity;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestScoreStandardRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.RoleService;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Services.TestScoreStandardService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LicenseTypeRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService;
using SEP490_SU25_G90.vn.edu.fpt.Services.EmailService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ResetCodeService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ScheduleSlotRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassReponsitory;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CourseRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CourseService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarStatusRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarStatusService;

namespace SEP490_SU25_G90.vn.edu.fpt.Commons
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ILearningMaterialRepository, LearningMaterialRepository>();
            services.AddScoped<ILearningMaterialService, LearningMaterialService>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ILearningApplicationRepository, LearningApplicationRepository>();
            services.AddScoped<ILearningApplicationService, LearningApplicationService>();

            services.AddScoped<ITestApplicationRepository, TestApplicationRepository>();
            services.AddScoped<ITestApplicationService, TestApplicationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IInstructorService, InstructorService>();

            services.AddScoped<ILicenseTypeRepository, LicenseTypeRepository>();
            services.AddScoped<ILicenseTypeService, LicenseTypeService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IroleService, RoleService>();

            services.AddScoped<ITestScoreStandardRepository, TestScoreStandardRepository>();
            services.AddScoped<ITestScoreStandardService, TestScoreStandardService>();

            services.AddScoped<ICarAssignmentRepository, CarAssignmentRepository>();
            services.AddScoped<ICarAssignmentService, CarAssignmentService>();

            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarService, CarService>();

            services.AddScoped<IPasswordHasher<Models.User>, PasswordHasher<Models.User>>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddSingleton<IResetCodeStorageService, ResetCodeStorageService>();

            services.AddScoped<IScheduleSlotRepository, ScheduleSlotRepository>();
            services.AddScoped<IScheduleSlotService, ScheduleSlotService>();

            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICarService, CarService>();

            services.AddScoped<ICarAssignmentRepository, CarAssignmentRepository>();
            services.AddScoped<ICarAssignmentService, CarAssignmentService>();

            // Class services - Dịch vụ quản lý lớp học
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IClassService, ClassService>();

            // Course services - Dịch vụ quản lý khóa học
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();

            //CarStatus Service
            services.AddScoped<ICarStatusRepository, CarStatusRepository>();
            services.AddScoped<ICarStatusService, CarStatusService>();

            return services;
        }
    }
}
