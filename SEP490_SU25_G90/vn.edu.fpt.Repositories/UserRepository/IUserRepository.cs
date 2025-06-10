using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository
{
    public interface IUserRepository
    {
        public List<User> GetAllUsers();
        public User GetById(int id);
        public List<User> GetByName(string name);
        public List<User> GetByEmail(string email);
        public User GetByUsername(string username);
        public void Create(User user);
        public void Update(User user);
        public List<User> GetByCccd(string cccd);
    }
}
