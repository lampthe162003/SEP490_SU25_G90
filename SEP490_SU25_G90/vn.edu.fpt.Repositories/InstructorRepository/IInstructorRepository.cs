using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository
{
    public interface IInstructorRepository
    {
        IQueryable<User> GetAllInstructors();
        IQueryable<User> GetInstructorsByName(IQueryable<User> query, string name);
        IQueryable<User> GetInstructorsByLicenceType(IQueryable<User> query, byte licenceTypeId);
        User? GetInstructorById(int id);
        void Create(User instructor);
        void Update(User instructor);
        void Delete(int id);
        void AddSpecialization(InstructorSpecialization specialization);
        void RemoveSpecialization(int instructorId, byte licenceTypeId);
        List<LicenceType> GetAllLicenceTypes();
    }
} 