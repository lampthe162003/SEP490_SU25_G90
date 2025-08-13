using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Commons.TestDataSeeding
{
    public class UserServiceSeeding
    {
        public List<User> SeedGetAllUsersItems()
        {
            return new List<User>
            {
                new User { Email = "test.learner@example.com", FirstName = "Trần", MiddleName = "Văn", LastName = "B"}
            };
        }
    }
}
