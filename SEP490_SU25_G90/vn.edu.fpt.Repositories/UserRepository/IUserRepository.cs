using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public IQueryable<User> GetAllUsers();
        public IQueryable<User> GetUsersByEmail(IQueryable<User> query, string email);
        public IQueryable<User> GetUsersByName(IQueryable<User> query, string name);
        public Task<User> Create(User user);
        public Task Update(User user);
        public Task<User> GetLoginDetails(string email, string password);
        public Task SaveChangesAsync();
        public Task<User> GetUserById(int id);
    }
}
