using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class InstructorListInformationResponse
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {MiddleName} {LastName}".Trim();
        public DateOnly? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImageUrl { get; set; }
        
        // Address information
        public string? AddressDisplay { get; set; }
        
        // CCCD information
        public string? CccdNumber { get; set; }
        
        // Specializations - các loại bằng giảng viên có thể dạy
        public List<LicenceTypeResponse> Specializations { get; set; } = new List<LicenceTypeResponse>();
        
        // Số học viên hiện tại
        public int StudentCount { get; set; }
    }
    
    public class LicenceTypeResponse
    {
        public byte LicenceTypeId { get; set; }
        public string LicenceCode { get; set; } = null!;
    }
} 