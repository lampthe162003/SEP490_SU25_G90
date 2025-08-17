using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public UserRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers() => await _context.Users
            .Include(u => u.Address)
            .Include(u => u.Cccd)
            .Include(u => u.HealthCertificate)
            .Include(u => u.InstructorSpecializations)
            //.Include(u => u.LearningApplicationInstructors)
            //.Include(u => u.LearningApplicationLearners)
            //.Include(u => u.MockTestResults)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .ToListAsync();

        public async Task Update(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetLoginDetails(string email, string password)
            => await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.Email.Equals(email) && u.Password.Equals(password))
            .FirstOrDefaultAsync();

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Cccd)
                .Include(u => u.HealthCertificate)
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Where(u => u.UserId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
        }

        public async Task SeedUserData()
        {
            _context.AddRange(
                new User
                {
                    UserId = 101,
                    Email = "test.learner@example.com",
                    Password = "test_data_1",
                    FirstName = "Trần",
                    MiddleName = "Văn",
                    LastName = "A",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1996-01-09")),
                    Gender = true,
                    Phone = "0123456789",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 101,
                            UserId = 101,
                            RoleId = 1
                        }
                    }
                },

                new User
                {
                    UserId = 102,
                    Email = "test.hr@example.com",
                    Password = "test_data_2",
                    FirstName = "Nguyễn",
                    MiddleName = "Thị",
                    LastName = "B",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1997-02-10")),
                    Gender = false,
                    Phone = "0111222333",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 102,
                            UserId = 102,
                            RoleId = 2
                        }
                    }
                },

                new User
                {
                    UserId = 103,
                    Email = "test.instructor@example.com",
                    Password = "test_data_3",
                    FirstName = "Phạm",
                    MiddleName = "Nhật",
                    LastName = "C",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1995-03-11")),
                    Gender = true,
                    Phone = "0444555666",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 103,
                            UserId = 103,
                            RoleId = 3
                        }
                    }
                },

                new User
                {
                    UserId = 104,
                    Email = "test.academicaffairs@example.com",
                    Password = "test_data_4",
                    FirstName = "Nguyễn",
                    MiddleName = "Thị",
                    LastName = "D",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1995-04-12")),
                    Gender = true,
                    Phone = "0777888999",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 104,
                            UserId = 104,
                            RoleId = 4
                        }
                    }
                }
            );
            _context.SaveChangesAsync();
        }
    }
}
