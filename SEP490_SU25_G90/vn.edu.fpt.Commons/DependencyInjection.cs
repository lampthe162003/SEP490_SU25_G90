using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.RoleService;
using Microsoft.AspNetCore.Identity;

namespace SEP490_SU25_G90.vn.edu.fpt.Commons
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ILearningApplicationRepository, LearningApplicationRepository>();
            services.AddScoped<ILearningApplicationService, LearningApplicationService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IInstructorRepository, InstructorRepository>();
            services.AddScoped<IInstructorService, InstructorService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IroleService, RoleService>();

            services.AddScoped<IPasswordHasher<Models.User>, PasswordHasher<Models.User>>();

            return services;
        }
    }
}
