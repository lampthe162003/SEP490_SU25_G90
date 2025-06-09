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

        public List<User> GetAllUsers() => _context.Users
            .Include(u => u.Address)
            .Include(u => u.Cccd)
            .Include(u => u.HealthCertificate)
            .Include(u => u.InstructorSpecializations)
            .Include(u => u.LearningApplicationInstructors)
            .Include(u => u.LearningApplicationLearners)
            .Include(u => u.MockTestResults)
            .Include(u => u.UserRoles)
            .ToList();

        public List<User> GetByEmail(string email) => _context.Users.Where(u => u.Email.Contains(email)).ToList();

        public User GetById(int id) => _context.Users.Find(id);

        public List<User> GetByName(string name)
            => _context.Users
            .Where(u => u.FirstName.Contains(name) || u.MiddleName.Contains(name) || u.LastName.Contains(name))
            .ToList();

        public User GetByUsername(string username) => _context.Users.Where(u => u.Username.Equals(u.Username)).FirstOrDefault();

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public List<User> GetByCccd(string cccd) => _context.Users
            .Include(u => u.Cccd)
            .Where(u => u.Cccd.CccdNumber.Contains(cccd))
            .ToList();
    }
}
