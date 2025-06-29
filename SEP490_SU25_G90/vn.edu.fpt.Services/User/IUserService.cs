using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.User
{
    public interface IUserService
    {
        public Task<IList<UserListInformationResponse>> GetAllUsers(string? name, string? email);
        public Task<LoginInformationResponse> GetLoginDetails(string email, string password);
        public Task CreateAccount(AccountCreationRequest request, byte roleId);
        public Task<List<LearnerUserResponse>> GetAllLearnersAsync();
        public Task<LearnerDetailResponse?> GetLearnerById(int id);
    }
}
