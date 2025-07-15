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

        public IQueryable<User> GetAllUsers() => _context.Users
            .Include(u => u.Address)
            .Include(u => u.Cccd)
            .Include(u => u.HealthCertificate)
            .Include(u => u.InstructorSpecializations)
            //.Include(u => u.LearningApplicationInstructors)
            //.Include(u => u.LearningApplicationLearners)
            //.Include(u => u.MockTestResults)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .AsQueryable();

        public IQueryable<User> GetUsersByEmail(IQueryable<User> query, string email)
            => query.Where(u => u.Email.Contains(email));

        public IQueryable<User> GetUsersByName(IQueryable<User> query, string name)
            => query.Where(u => u.FirstName.Contains(name) ||
                u.MiddleName.Contains(name) ||
                u.LastName.Contains(name));

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
            return await _context.Users.FindAsync(id);
        }
    }
}
