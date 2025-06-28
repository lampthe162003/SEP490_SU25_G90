using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public InstructorRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllInstructors()
        {
            // Lấy users có InstructorSpecializations (là instructors)
            return _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Include(u => u.Cccd)
                .Include(u => u.InstructorSpecializations)
                    .ThenInclude(ins => ins.LicenceType)
                //.Include(u => u.LearningApplicationInstructors)
                    //.ThenInclude(la => la.Learner)
                .Where(u => u.InstructorSpecializations.Any())
                .AsQueryable();
        }

        public IQueryable<User> GetInstructorsByName(IQueryable<User> query, string name)
        {
            return query.Where(u => 
                u.FirstName.Contains(name) ||
                u.MiddleName.Contains(name) ||
                u.LastName.Contains(name) ||
                (u.FirstName + " " + u.MiddleName + " " + u.LastName).Contains(name));
        }

        public IQueryable<User> GetInstructorsByLicenceType(IQueryable<User> query, byte licenceTypeId)
        {
            return query.Where(u => u.InstructorSpecializations.Any(ins => ins.LicenceTypeId == licenceTypeId));
        }

        public User? GetInstructorById(int id)
        {
            return _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Include(u => u.Cccd)
                .Include(u => u.InstructorSpecializations)
                    .ThenInclude(ins => ins.LicenceType)
                //.Include(u => u.LearningApplicationInstructors)
                .FirstOrDefault(u => u.UserId == id);
        }

        public void Create(User instructor)
        {
            _context.Users.Add(instructor);
            _context.SaveChanges();
        }

        public void Update(User instructor)
        {
            _context.Users.Update(instructor);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var instructor = _context.Users.Find(id);
            if (instructor != null)
            {
                _context.Users.Remove(instructor);
                _context.SaveChanges();
            }
        }

        public void AddSpecialization(InstructorSpecialization specialization)
        {
            _context.InstructorSpecializations.Add(specialization);
            _context.SaveChanges();
        }

        public void RemoveSpecialization(int instructorId, byte licenceTypeId)
        {
            var specialization = _context.InstructorSpecializations
                .FirstOrDefault(ins => ins.InstructorId == instructorId && ins.LicenceTypeId == licenceTypeId);
            
            if (specialization != null)
            {
                _context.InstructorSpecializations.Remove(specialization);
                _context.SaveChanges();
            }
        }

        public List<LicenceType> GetAllLicenceTypes()
        {
            return _context.LicenceTypes.ToList();
        }
    }
} 