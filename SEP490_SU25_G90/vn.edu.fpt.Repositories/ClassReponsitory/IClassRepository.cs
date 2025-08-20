using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassReponsitory
{
    /// <summary>
    /// Interface định nghĩa các phương thức truy cập dữ liệu cho Class
    /// </summary>
    public interface IClassRepository
    {
        /// <summary>
        /// Lấy danh sách lớp học theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchRequest">Điều kiện tìm kiếm</param>
        /// <returns>Danh sách lớp học</returns>
        Task<IEnumerable<ClassListResponse>> GetClassesAsync(ClassSearchRequest searchRequest);
        
        /// <summary>
        /// Đếm tổng số lớp học theo điều kiện tìm kiếm
        /// </summary>
        /// <param name="searchRequest">Điều kiện tìm kiếm</param>
        /// <returns>Tổng số lớp học</returns>
        Task<int> CountClassesAsync(ClassSearchRequest searchRequest);
        
        /// <summary>
        /// Lấy tất cả loại bằng lái
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

        /// <summary>
        /// Lấy thông tin lớp học theo ID
        /// </summary>
        /// <param name="classId">ID của lớp học</param>
        /// <returns>Thông tin lớp học hoặc null</returns>
        Task<Class?> GetClassByIdAsync(int classId);

        /// <summary>
        /// Lấy danh sách thành viên trong lớp
        /// </summary>
        /// <param name="classId">ID của lớp học</param>
        /// <returns>Danh sách thành viên lớp học</returns>
        Task<List<ClassMember>> GetClassMembersAsync(int classId);
    }
}
