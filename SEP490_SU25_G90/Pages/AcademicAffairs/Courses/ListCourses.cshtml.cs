using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Course;
using SEP490_SU25_G90.vn.edu.fpt.Services.CourseService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Courses
{
    [Authorize(Roles = "academic affairs")]
    public class ListCoursesModel : PageModel
    {
        private readonly ICourseService _courseService;

        public ListCoursesModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty(SupportsGet = true)]
        public CourseSearchRequest SearchRequest { get; set; } = new();

        public Pagination<CourseListResponse> Courses { get; set; } = new();
        public List<SelectListItem> LicenceTypes { get; set; } = new();
        public List<SelectListItem> StatusOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public bool HasSearchParameters()
        {
            return !string.IsNullOrEmpty(SearchRequest.CourseName)
                   || SearchRequest.LicenceTypeId.HasValue
                   || !string.IsNullOrEmpty(SearchRequest.Status)
                   || SearchRequest.StartDate.HasValue
                   || SearchRequest.EndDate.HasValue;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                ValidateSearchRequest();
                Courses = await _courseService.GetCoursesAsync(SearchRequest);
                await LoadDropdownDataAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tải dữ liệu: " + ex.Message);
                Courses = new Pagination<CourseListResponse>();
                await LoadDropdownDataAsync();
            }
        }

        private void ValidateSearchRequest()
        {
            if (SearchRequest.PageNumber <= 0) SearchRequest.PageNumber = 1;
            if (SearchRequest.PageSize <= 0) SearchRequest.PageSize = 10;

            if (SearchRequest.StartDate.HasValue && SearchRequest.EndDate.HasValue &&
                SearchRequest.StartDate > SearchRequest.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                SearchRequest.StartDate = null;
                SearchRequest.EndDate = null;
            }

            if (SearchRequest.LicenceTypeId.HasValue && SearchRequest.LicenceTypeId <= 0)
            {
                SearchRequest.LicenceTypeId = null;
            }
        }

        private async Task LoadDropdownDataAsync()
        {
            var licenceTypes = await _courseService.GetAllLicenceTypesAsync();
            LicenceTypes = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "-- Loại bằng --" }
            };
            LicenceTypes.AddRange(licenceTypes.Select(lt => new SelectListItem
            {
                Value = lt.LicenceTypeId.ToString(),
                Text = lt.LicenceCode
            }));

            StatusOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "-- Trạng thái --" },
                new SelectListItem { Value = "Chưa bắt đầu", Text = "Chưa bắt đầu" },
                new SelectListItem { Value = "Đang diễn ra", Text = "Đang diễn ra" },
                new SelectListItem { Value = "Đã kết thúc", Text = "Đã kết thúc" }
            };
        }

        public string CreatePagingUrl(int pageNumber)
        {
            var routeValues = new Dictionary<string, object?>
            {
                { "SearchRequest.PageNumber", pageNumber },
                { "SearchRequest.PageSize", SearchRequest.PageSize }
            };

            if (!string.IsNullOrEmpty(SearchRequest.CourseName))
                routeValues.Add("SearchRequest.CourseName", SearchRequest.CourseName);
            if (SearchRequest.LicenceTypeId.HasValue)
                routeValues.Add("SearchRequest.LicenceTypeId", SearchRequest.LicenceTypeId);
            if (!string.IsNullOrEmpty(SearchRequest.Status))
                routeValues.Add("SearchRequest.Status", SearchRequest.Status);
            if (SearchRequest.StartDate.HasValue)
                routeValues.Add("SearchRequest.StartDate", SearchRequest.StartDate.Value.ToString("yyyy-MM-dd"));
            if (SearchRequest.EndDate.HasValue)
                routeValues.Add("SearchRequest.EndDate", SearchRequest.EndDate.Value.ToString("yyyy-MM-dd"));

            return Url.Page("./ListCourses", routeValues) ?? "#";
        }

        public string CreateResetUrl()
        {
            return Url.Page("./ListCourses") ?? "#";
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // Lấy chi tiết để kiểm tra trạng thái dựa vào ngày
                var course = await _courseService.GetDetailAsync(id);
                if (course == null)
                {
                    TempData["ErrorMessage"] = "Khóa học không tồn tại.";
                    return RedirectToPage();
                }

                var today = DateOnly.FromDateTime(DateTime.Now);
                if (course.StartDate.HasValue && today >= course.StartDate)
                {
                    TempData["ErrorMessage"] = "Chỉ được xóa khóa học khi chưa bắt đầu.";
                    return RedirectToPage();
                }

                await _courseService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Xóa khóa học thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Không thể xóa khóa học: " + ex.Message;
            }

            return RedirectToPage();
        }
    }
}

