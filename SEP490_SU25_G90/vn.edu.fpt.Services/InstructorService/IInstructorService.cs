using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService
{
    public interface IInstructorService
    {
        IList<InstructorListInformationResponse> GetAllInstructors(string? name = null, byte? licenceTypeId = null);
        InstructorListInformationResponse? GetInstructorById(int id);
        void CreateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor);
        void UpdateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor);
        void UpdateInstructorInfo(int instructorId, UpdateInstructorRequest request);
        void DeleteInstructor(int id);
        void AddSpecialization(int instructorId, byte licenceTypeId);
        void RemoveSpecialization(int instructorId, byte licenceTypeId);
        List<LicenceTypeResponse> GetAllLicenceTypes();
        
        // Learner methods
        Task<List<LearnerUserResponse>> GetAllLearnersAsync(string? searchString = null);
    }
}