using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;

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
        public Task<UserDetailsInformationResponse> GetUserDetailsAsync(int userId);
        public Task<LoginInformationResponse> GetLoginDetailsByEmail(string email);
        public Task ResetPasswordAsync(string email, string newPassword);
        public Task<bool> DoesUserWithEmailExist(string email);
        public Task<bool> DoesUserWithCccdExist(string cccdNumber);
        public Task<bool> DoesUserWithPhoneExist(string phoneNumber);
        public Task<bool> DoesUserWithCccdExistExcludingUser(string cccdNumber, int excludeUserId);
        public Task<bool> DoesUserWithPhoneExistExcludingUser(string phoneNumber, int excludeUserId);
        public Task<List<UserListInformationResponse>> GetUsersByRole(byte roleId);
        public Task UpdateStaffAsync(UpdateStaffRequest request);
    }
}
