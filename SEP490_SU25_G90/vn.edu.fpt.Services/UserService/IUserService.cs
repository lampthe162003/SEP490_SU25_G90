using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.UserService
{
    public interface IUserService
    {
        public Task<IList<UserListInformationResponse>> GetAllUsers(string? name, string? email);
        public Task<LoginInformationResponse> GetLoginDetails(string email, string password);
        public Task CreateAccount(AccountCreationRequest request, byte roleId);
        public Task<List<LearnerUserResponse>> GetAllLearnersAsync();
        public Task<LearnerDetailResponse?> GetLearnerById(int id);
        public Task<string> CreateLearnerAsync(CreateLearnerRequest request);
        public Task UpdateLearnerInfoAsync(int userId, UpdateLearnerRequest request);
        public Task UpdatePasswordAsync(int userId, string newPassword);
        public Task<UserListInformationResponse> GetUserDetailsAsync(int userId);
        public Task<LoginInformationResponse> GetLoginDetailsByEmail(string email);
        public Task ResetPasswordAsync(string email, string newPassword);
        public Task<bool> DoesUserWithEmailExist(string email);
    }
}
