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
using SEP490_SU25_G90.vn.edu.fpt.Services.User;
using SEP490_SU25_G90.vn.edu.fpt.Services.LicenseTypeService;
using SEP490_SU25_G90.vn.edu.fpt.Services.EmailService;

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

            services.AddScoped<IPasswordHasher<Models.User>, PasswordHasher<Models.User>>();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
