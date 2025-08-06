using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassReponsitory;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.ClassService
{
    /// <summary>
    /// Service implementation cho Class business logic
    /// </summary>
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        /// <summary>
        /// Lấy danh sách lớp học với phân trang và tìm kiếm
        /// </summary>
        public async Task<Pagination<ClassListResponse>> GetClassesAsync(ClassSearchRequest searchRequest)
        {
            // Validate input parameters
            if (searchRequest.PageNumber <= 0)
                searchRequest.PageNumber = 1;
            
            if (searchRequest.PageSize <= 0)
                searchRequest.PageSize = 10;

            // Lấy dữ liệu từ repository
            var classes = await _classRepository.GetClassesAsync(searchRequest);
            var totalCount = await _classRepository.CountClassesAsync(searchRequest);

            // Tạo pagination response
            return new Pagination<ClassListResponse>
            {
                Data = classes.ToList(),
                Total = totalCount,
                TotalPage = (int)Math.Ceiling((double)totalCount / searchRequest.PageSize)
            };
        }

        /// <summary>
        /// Lấy tất cả loại bằng lái để hiển thị trong dropdown
        /// </summary>
        public async Task<IEnumerable<LicenceType>> GetAllLicenceTypesAsync()
        {
            return await _classRepository.GetAllLicenceTypesAsync();
        }
    }
}
