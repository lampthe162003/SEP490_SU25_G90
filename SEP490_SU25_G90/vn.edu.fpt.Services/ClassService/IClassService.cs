using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ClassService
{
    /// <summary>
    /// Interface định nghĩa các business logic cho Class
    /// </summary>
    public interface IClassService
    {
        /// <summary>
        /// Lấy danh sách lớp học với phân trang và tìm kiếm
        /// </summary>
        /// <param name="searchRequest">Điều kiện tìm kiếm</param>
        /// <returns>Danh sách lớp học với thông tin phân trang</returns>
        Task<Pagination<ClassListResponse>> GetClassesAsync(ClassSearchRequest searchRequest);
        
        /// <summary>
        /// Lấy tất cả loại bằng lái để hiển thị trong dropdown
        /// </summary>
        /// <returns>Danh sách loại bằng lái</returns>
        Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync();
        
        /// <summary>
        /// Lấy chi tiết lớp học theo ID
        /// </summary>
        /// <param name="classId">ID của lớp học</param>
        /// <returns>Chi tiết lớp học</returns>
        Task<ClassDetailResponse?> GetClassDetailAsync(int classId);
        
        /// <summary>
        /// Kiểm tra lớp học có tồn tại không
        /// </summary>
        /// <param name="classId">ID của lớp học</param>
        /// <returns>True nếu tồn tại</returns>
        Task<bool> ClassExistsAsync(int classId);
    }
}
