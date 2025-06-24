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

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public IQueryable<User> GetAllUsers() => _context.Users
            .Include(u => u.Address)
            .Include(u => u.Cccd)
            .Include(u => u.HealthCertificate)
            .Include(u => u.InstructorSpecializations)
            .Include(u => u.LearningApplicationInstructors)
            .Include(u => u.LearningApplicationLearners)
            .Include(u => u.MockTestResults)
            .Include(u => u.UserRoles)
            .AsQueryable();

        public IQueryable<User> GetUsersByEmail(IQueryable<User> query, string email)
            => query.Where(u => u.Email.Contains(email));

        public IQueryable<User> GetUsersByName(IQueryable<User> query, string name)
            => query.Where(u => u.FirstName.Contains(name) ||
                u.MiddleName.Contains(name) ||
                u.LastName.Contains(name));

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public User GetLoginDetails(string email, string password)
            => _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(password))
            .FirstOrDefault();
    }
}
