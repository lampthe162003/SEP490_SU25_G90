using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto
{
    public class UserDetailsInformationResponse
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Fullname { get; set; }
        public DateOnly Dob { get; set; }
        public bool? Gender { get; set; }
        public int CccdId { get; set; }
        public int HealthCertificateId { get; set; }
        public string? Phone { get; set; }
        public int AddressId { get; set; }

        public virtual Cccd? Cccd { get; set; }
        public virtual HealthCertificate? HealthCertificate { get; set; }
        public virtual Address? Address { get; set; }
    }
}
