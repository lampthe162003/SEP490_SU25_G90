using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment
{
    public class CarAssignmentResponse
    {
        public int AssignmentId { get; set; }
        
        // Thông tin xe
        public int CarId { get; set; }
        public string? LicensePlate { get; set; }
        public string? CarMake { get; set; }
        public string? CarModel { get; set; }
        
        // Thông tin giáo viên
        public int InstructorId { get; set; }
        public string? InstructorFullName { get; set; }
        public string? InstructorPhone { get; set; }
        public string? InstructorEmail { get; set; }
        
        // Thông tin loại bằng giáo viên có thể dạy
        public List<LicenceTypeInfo> InstructorLicenceTypes { get; set; } = new List<LicenceTypeInfo>();
        
        // Thông tin ca học
        public int SlotId { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? SlotDisplayName { get; set; }
        
        // Thông tin lịch
        public DateOnly? ScheduleDate { get; set; }
        
        // Trạng thái thuê xe
        public byte? CarStatus { get; set; }
        public string? CarStatusDisplay { get; set; }
        
        // Thông tin tìm kiếm và hiển thị
        public bool CanRent { get; set; }
    }
    
    public class LicenceTypeInfo
    {
        public byte LicenceTypeId { get; set; }
        public string? LicenceCode { get; set; }
    }
} 