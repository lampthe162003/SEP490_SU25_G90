using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassTimeService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Classes
{
    /// <summary>
    /// Page model cho màn hình chi tiết lớp học (AcademicAffairs)
    /// </summary>
    [Authorize(Roles = "academic affairs")]
    public class ClassDetailsModel : PageModel
    {
        private readonly IClassService _classService;
        private readonly IClassTimeService _classTimeService;

        public ClassDetailsModel(IClassService classService, IClassTimeService classTimeService)
        {
            _classService = classService;
            _classTimeService = classTimeService;
        }

        // Properties để hiển thị dữ liệu
        public ClassDetailResponse ClassDetail { get; set; } = new ClassDetailResponse();
        public bool ClassNotFound { get; set; } = false;
        public List<ClassTime> ClassSchedules { get; set; } = new List<ClassTime>();

        // URL Parameters
        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        // Pagination cho danh sách học viên
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; } = 1;
        public List<ClassMemberResponse> PagedMembers { get; set; } = new List<ClassMemberResponse>();

        /// <summary>
        /// Xử lý GET request - Hiển thị chi tiết lớp học
        /// </summary>
        /// <param name="page">Trang hiện tại cho danh sách học viên</param>
        public async Task<IActionResult> OnGetAsync(int page = 1)
        {
            // Debug: Log received parameters
            Console.WriteLine($"ClassDetails - Id: {Id}, Page: {page}");

            // Validate input
            if (!Id.HasValue || Id.Value <= 0)
            {
                Console.WriteLine($"ClassDetails - Invalid Id: {Id}");
                return NotFound();
            }

            try
            {
                // Kiểm tra lớp học có tồn tại không
                var classExists = await _classService.ClassExistsAsync(Id.Value);
                if (!classExists)
                {
                    ClassNotFound = true;
                    return Page();
                }

                // Lấy chi tiết lớp học
                var classDetail = await _classService.GetClassDetailAsync(Id.Value);
                if (classDetail == null)
                {
                    ClassNotFound = true;
                    return Page();
                }

                ClassDetail = classDetail;

                // Load class schedules
                ClassSchedules = await _classTimeService.GetClassTimesAsync(Id.Value);

                // Xử lý phân trang cho danh sách học viên
                SetupPagination(page);

                return Page();
            }
            catch (Exception ex)
            {
                // Log error
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
                ClassNotFound = true;
                return Page();
            }
        }

        /// <summary>
        /// Thiết lập phân trang cho danh sách học viên
        /// </summary>
        private void SetupPagination(int page)
        {
            CurrentPage = page > 0 ? page : 1;

            var totalMembers = ClassDetail.Members.Count;
            TotalPages = (int)Math.Ceiling((double)totalMembers / PageSize);

            // Đảm bảo trang hiện tại hợp lệ
            if (CurrentPage > TotalPages && TotalPages > 0)
                CurrentPage = TotalPages;

            // Lấy dữ liệu cho trang hiện tại
            PagedMembers = ClassDetail.Members
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        /// <summary>
        /// Tạo URL cho phân trang
        /// </summary>
        public string CreatePagingUrl(int pageNumber)
        {
            var classId = Id ?? ClassDetail.ClassId;
            if (pageNumber <= 1)
            {
                return $"/AcademicAffairs/Classes/ClassDetails/{classId}";
            }
            return $"/AcademicAffairs/Classes/ClassDetails/{classId}?page={pageNumber}";
        }

        /// <summary>
        /// Tạo URL quay lại danh sách
        /// </summary>
        public string CreateBackUrl()
        {
            return "/AcademicAffairs/Classes/ListClasses";
        }

        /// <summary>
        /// Tạo URL cập nhật lớp học
        /// </summary>
        public string CreateUpdateUrl()
        {
            var classId = Id ?? ClassDetail.ClassId;
            return $"/AcademicAffairs/Classes/UpdateClasses/{classId}";
        }

        /// <summary>
        /// Tạo URL để thêm học viên cho lớp
        /// </summary>
        public string CreateAddMembersUrl()
        {
            var classId = Id ?? ClassDetail.ClassId;
            return $"/AcademicAffairs/Classes/AddMembers?classId={classId}";
        }

        /// <summary>
        /// Tạo URL xem thông tin giáo viên
        /// </summary>
        public string CreateInstructorUrl()
        {
            if (ClassDetail.InstructorId.HasValue)
            {
                return $"/AcademicAffairs/LearningProfile/InstructorDetail?id={ClassDetail.InstructorId.Value}";
            }
            return "#";
        }

        /// <summary>
        /// Tạo URL xem chi tiết học viên
        /// </summary>
        public string CreateLearnerUrl(int learnerId)
        {
            return $"/AcademicAffairs/LearningApplications/Detail/{learnerId}";
        }

        /// <summary>
        /// Format hiển thị trạng thái với badge class
        /// </summary>
        public string GetStatusBadgeClass()
        {
            return ClassDetail.Status switch
            {
                "Chưa bắt đầu" => "bg-warning text-dark",
                "Đang học" => "bg-success",
                "Đã học xong" => "bg-secondary",
                _ => "bg-light text-dark"
            };
        }

        /// <summary>
        /// Kiểm tra có hiển thị phân trang không
        /// </summary>
        public bool ShouldShowPagination => TotalPages > 1;

        /// <summary>
        /// Lấy tên thứ trong tuần
        /// </summary>
        public string GetDayName(byte dayNumber)
        {
            return dayNumber switch
            {
                2 => "Thứ 2",
                3 => "Thứ 3",
                4 => "Thứ 4",
                5 => "Thứ 5",
                6 => "Thứ 6",
                7 => "Thứ 7",
                1 => "Chủ nhật",
                _ => ""
            };
        }

        /// <summary>
        /// Nhóm lịch học theo ngày
        /// </summary>
        public Dictionary<byte, List<ClassTime>> GetGroupedSchedules()
        {
            return ClassSchedules
                .GroupBy(cs => cs.Thu)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.OrderBy(s => s.Slot?.StartTime).ToList());
        }

        /// <summary>
        /// Lấy class CSS cho badge trạng thái học dựa trên tên trạng thái
        /// </summary>
        public string GetLearningStatusBadgeClass(string? learningStatus)
        {
            return learningStatus switch
            {
                "Đang học" => "bg-success",
                "Bảo lưu" => "bg-warning text-dark",
                "Học lại" => "bg-danger",
                "Hoàn thành" => "bg-primary",
                _ => "bg-secondary"
            };
        }
    }
}