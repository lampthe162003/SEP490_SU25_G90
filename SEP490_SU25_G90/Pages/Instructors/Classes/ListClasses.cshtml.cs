using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;
using System.Security.Claims;

namespace SEP490_SU25_G90.Pages.Instructors.Classes
{
    /// <summary>
    /// Page model cho màn hình danh sách lớp học (instructor)
    /// </summary>
    //[Authorize(Roles = "instructor")]
    public class ListClassesModel : PageModel
    {
        private readonly IClassService _classService;

        public ListClassesModel(IClassService classService)
        {
            _classService = classService;
        }

        // Properties để bind dữ liệu với form
        [BindProperty(SupportsGet = true)]
        public ClassSearchRequest SearchRequest { get; set; } = new ClassSearchRequest();

        // Dữ liệu hiển thị
        public Pagination<ClassListResponse> Classes { get; set; } = new Pagination<ClassListResponse>();
        public List<SelectListItem> LicenceTypes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusOptions { get; set; } = new List<SelectListItem>();

        /// <summary>
        /// Xử lý GET request - Hiển thị danh sách lớp học và xử lý tìm kiếm
        /// </summary>
        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        /// <summary>
        /// Kiểm tra xem có tham số tìm kiếm nào không
        /// </summary>
        public bool HasSearchParameters()
        {
            return !string.IsNullOrEmpty(SearchRequest.ClassName) ||
                   SearchRequest.LicenceTypeId.HasValue ||
                   !string.IsNullOrEmpty(SearchRequest.Status) ||
                   SearchRequest.StartDate.HasValue ||
                   SearchRequest.EndDate.HasValue;
        }

        /// <summary>
        /// Load dữ liệu cho trang
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                // Get instructor ID from JWT claims
                //var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userIdClaim = "7"; // --> Fix cứng tạm khi chưa login đc
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    ModelState.AddModelError("", "Không tìm thấy thông tin giảng viên.");
                    Classes = new Pagination<ClassListResponse>();
                    await LoadDropdownDataAsync();
                    return;
                }

                // Parse instructor ID
                if (!int.TryParse(userIdClaim, out int instructorId))
                {
                    ModelState.AddModelError("", "Thông tin giảng viên không hợp lệ.");
                    Classes = new Pagination<ClassListResponse>();
                    await LoadDropdownDataAsync();
                    return;
                }

                // Validate search parameters
                ValidateSearchRequest();

                // Set instructor ID in search request to filter only this instructor's classes
                SearchRequest.InstructorId = instructorId;

                // Load danh sách lớp học
                Classes = await _classService.GetClassesAsync(SearchRequest);

                // Load dropdown data
                await LoadDropdownDataAsync();
            }
            catch (Exception ex)
            {
                // Log error và hiển thị thông báo lỗi
                ModelState.AddModelError("", "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
                Classes = new Pagination<ClassListResponse>(); // Khởi tạo rỗng để tránh null reference
                
                // Load dropdown data để tránh null reference
                await LoadDropdownDataAsync();
            }
        }

        /// <summary>
        /// Validate các tham số tìm kiếm
        /// </summary>
        private void ValidateSearchRequest()
        {
            // Validate page number
            if (SearchRequest.PageNumber <= 0)
                SearchRequest.PageNumber = 1;

            // Validate page size
            if (SearchRequest.PageSize <= 0)
                SearchRequest.PageSize = 10;

            // Validate date range
            if (SearchRequest.StartDate.HasValue && SearchRequest.EndDate.HasValue)
            {
                if (SearchRequest.StartDate > SearchRequest.EndDate)
                {
                    ModelState.AddModelError("", "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                    SearchRequest.StartDate = null;
                    SearchRequest.EndDate = null;
                }
            }

            // Validate licence type
            if (SearchRequest.LicenceTypeId.HasValue && SearchRequest.LicenceTypeId <= 0)
            {
                SearchRequest.LicenceTypeId = null;
            }
        }

        /// <summary>
        /// Load dữ liệu cho các dropdown
        /// </summary>
        private async Task LoadDropdownDataAsync()
        {
            // Load loại bằng lái
            var licenceTypes = await _classService.GetAllLicenceTypesAsync();
            LicenceTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Tất cả loại bằng --" }
            };
            LicenceTypes.AddRange(licenceTypes.Select(lt => new SelectListItem
            {
                Value = lt.LicenceTypeId.ToString(),
                Text = lt.LicenceCode
            }));

            // Load trạng thái (fix cứng theo yêu cầu)
            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Tất cả trạng thái --" },
                new SelectListItem { Value = "Chưa bắt đầu", Text = "Chưa bắt đầu" },
                new SelectListItem { Value = "Đang học", Text = "Đang học" },
                new SelectListItem { Value = "Đã học xong", Text = "Đã học xong" }
            };
        }

        /// <summary>
        /// Tạo URL cho phân trang
        /// </summary>
        public string CreatePagingUrl(int pageNumber)
        {
            var routeValues = new Dictionary<string, object?>
            {
                { "SearchRequest.PageNumber", pageNumber },
                { "SearchRequest.PageSize", SearchRequest.PageSize }
            };

            // Chỉ thêm các tham số có giá trị
            if (!string.IsNullOrEmpty(SearchRequest.ClassName))
                routeValues.Add("SearchRequest.ClassName", SearchRequest.ClassName);

            if (SearchRequest.LicenceTypeId.HasValue)
                routeValues.Add("SearchRequest.LicenceTypeId", SearchRequest.LicenceTypeId);

            if (!string.IsNullOrEmpty(SearchRequest.Status))
                routeValues.Add("SearchRequest.Status", SearchRequest.Status);

            if (SearchRequest.StartDate.HasValue)
                routeValues.Add("SearchRequest.StartDate", SearchRequest.StartDate.Value.ToString("yyyy-MM-dd"));

            if (SearchRequest.EndDate.HasValue)
                routeValues.Add("SearchRequest.EndDate", SearchRequest.EndDate.Value.ToString("yyyy-MM-dd"));

            return Url.Page("./ListClasses", routeValues) ?? "#";
        }

        /// <summary>
        /// Tạo URL reset tìm kiếm
        /// </summary>
        public string CreateResetUrl()
        {
            return Url.Page("./ListClasses") ?? "#";
        }
    }
}
