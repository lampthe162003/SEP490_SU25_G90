using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllUsers();
        public Task<User> Create(User user);
        public Task Update(User user);
        public Task<User> GetLoginDetails(string email, string password);
        public Task SaveChangesAsync();
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByEmail(string email);
        public Task SeedUserData();
    }
}
