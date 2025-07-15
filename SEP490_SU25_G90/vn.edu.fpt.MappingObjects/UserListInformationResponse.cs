using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class UserListInformationResponse
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public DateOnly? Dob { get; set; }

        public bool? Gender { get; set; }

        public string? Phone { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
