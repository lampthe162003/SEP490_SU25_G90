using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService
{
    public interface IInstructorService
    {
        IList<InstructorListInformationResponse> GetAllInstructors(string? name = null, byte? licenceTypeId = null, byte? courseId = null);
        InstructorListInformationResponse? GetInstructorById(int id);
        void CreateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor);
        Task<string> CreateInstructorAsync(CreateInstructorRequest request);
        void UpdateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor);
        Task UpdateInstructorInfoAsync(int instructorId, UpdateInstructorRequest request);
        void DeleteInstructor(int id);
        void AddSpecialization(int instructorId, byte licenceTypeId);
        void RemoveSpecialization(int instructorId, byte licenceTypeId);
        List<LicenceTypeResponse> GetAllLicenceTypes();

        // Learner methods
        Task<List<LearnerUserResponse>> GetAllLearnersAsync(string? searchString = null);
        Task<bool> UpdateLearnerScoresAsync(int learningId, int? theory, int? simulation, int? obstacle, int? practical);
        Task<List<LearningApplicationsResponse>> GetLearningApplicationsByInstructorAsync(int instructorId);
        Task<LearningApplicationsResponse?> GetLearningApplicationDetailAsync(int learningId);



    }
}