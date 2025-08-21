using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;
using SEP490_SU25_G90.vn.edu.fpt.Services.CourseService;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Classes
{
    [Authorize(Roles = "academic affairs")]
    public class CreateClassesModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly ILearningApplicationService _learningService;
        private readonly IScheduleSlotService _scheduleSlotService;
        private readonly ICourseService _courseService;
        private readonly IClassService _classService;

        public CreateClassesModel(
            IInstructorService instructorService,
            ILearningApplicationService learningService,
            IScheduleSlotService scheduleSlotService,
            ICourseService courseService,
            IClassService classService)
        {
            _instructorService = instructorService;
            _learningService = learningService;
            _scheduleSlotService = scheduleSlotService;
            _courseService = courseService;
            _classService = classService;
        }

        [BindProperty]
        public string? ClassName { get; set; }

        [BindProperty]
        public int? InstructorId { get; set; }

        [BindProperty]
        public int? SelectedCourseId { get; set; }
        public int MaxStudents = 5;

        [BindProperty]
        public List<int> SelectedLearnerIds { get; set; } = new();

        [BindProperty]
        public List<string> SelectedSchedules { get; set; } = new();

        public List<SelectListItem> Courses { get; set; } = new();
        public List<SelectListItem> Instructors { get; set; } = new();
        public List<ScheduleSlot> ScheduleSlots { get; set; } = new();

        public List<WaitingLearnerResponse> WaitingLearners { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? CourseId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();
            await LoadScheduleSlotsAsync();

            if (CourseId.HasValue)
            {
                SelectedCourseId = CourseId;
            }

            // Nếu chưa có khóa được chọn, chọn mặc định là khóa đầu tiên
            if (!SelectedCourseId.HasValue && Courses.Any())
            {
                if (int.TryParse(Courses.First().Value, out var firstCourseId))
                {
                    SelectedCourseId = firstCourseId;
                }
            }

            // Load waiting learners and generate class name based on selected course
            if (SelectedCourseId.HasValue)
            {
                await LoadWaitingLearnersByCourseAsync(SelectedCourseId.Value);
                ClassName = await _classService.GenerateClassNameAsync(SelectedCourseId.Value);
            }
            else
            {
                await LoadWaitingLearnersAsync();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Load data for page reload if needed
            await LoadDropdownsAsync();
            await LoadScheduleSlotsAsync();

            // If course changed, reload the page with new course data
            if (Request.Form["CourseChanged"].ToString() == "true")
            {
                if (SelectedCourseId.HasValue)
                {
                    await LoadWaitingLearnersByCourseAsync(SelectedCourseId.Value);
                    ClassName = await _classService.GenerateClassNameAsync(SelectedCourseId.Value);
                }
                else
                {
                    await LoadWaitingLearnersAsync();
                }
                return Page();
            }

            if (!SelectedCourseId.HasValue)
            {
                ModelState.AddModelError(nameof(SelectedCourseId), "Vui lòng chọn khóa học");
            }

            if (!ModelState.IsValid)
            {
                if (SelectedCourseId.HasValue)
                {
                    await LoadWaitingLearnersByCourseAsync(SelectedCourseId.Value);
                    ClassName = await _classService.GenerateClassNameAsync(SelectedCourseId.Value);
                }
                else
                {
                    await LoadWaitingLearnersAsync();
                }
                return Page();
            }

            try
            {
                // Bỏ qua input từ client, luôn generate tên lớp theo quy tắc để chống chỉnh sửa
                ClassName = await _classService.GenerateClassNameAsync(SelectedCourseId.Value);

                var newClass = new vn.edu.fpt.Models.Class
                {
                    ClassName = ClassName,
                    InstructorId = InstructorId,
                    CourseId = SelectedCourseId
                };

                var classId = await _classService.CreateClassAsync(newClass, SelectedLearnerIds, SelectedSchedules);

                TempData["SuccessMessage"] = "Tạo lớp học thành công";
                return RedirectToPage("/AcademicAffairs/Classes/ClassDetails", new { id = classId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo lớp: " + ex.Message);
                if (SelectedCourseId.HasValue)
                {
                    await LoadWaitingLearnersByCourseAsync(SelectedCourseId.Value);
                    ClassName = await _classService.GenerateClassNameAsync(SelectedCourseId.Value);
                }
                else
                {
                    await LoadWaitingLearnersAsync();
                }
                return Page();
            }
        }

        // Remove AJAX endpoints - using MVC pattern with page reload instead

        private async Task LoadDropdownsAsync()
        {
            // Load courses with student count using CourseService
            var coursesWithStudentCount = await _courseService.GetCoursesWithStudentCountAsync();

            Courses = coursesWithStudentCount
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.DisplayText
                })
                .ToList();
            var instructorDtos = new List<InstructorListInformationResponse>();
            if (SelectedCourseId.HasValue)
            {
                // Get course to get its license type from the courses list
                var selectedCourse = coursesWithStudentCount.FirstOrDefault(c => c.CourseId == SelectedCourseId.Value);
                if (selectedCourse?.LicenceTypeId != null)
                {
                    // Get instructors by license type
                    var instructorsByLicense = _instructorService.GetAllInstructors(licenceTypeId: selectedCourse.LicenceTypeId);
                    if (instructorsByLicense != null)
                    {
                        instructorDtos.AddRange(instructorsByLicense);
                    }
                }
            }

            if (!instructorDtos.Any())
            {
                // Fallback to all instructors if no specific ones found
                var allInstructors = _instructorService.GetAllInstructors();
                if (allInstructors != null)
                {
                    instructorDtos.AddRange(allInstructors);
                }
            }

            Instructors = instructorDtos
                .Select(i => new SelectListItem
                {
                    Value = i.UserId.ToString(),
                    Text = string.Join(" ", new[] { i.FirstName, i.MiddleName, i.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)))
                }).ToList();
        }

        private async Task LoadWaitingLearnersAsync()
        {
            WaitingLearners = await _learningService.GetWaitingLearnersAsync();
        }

        private async Task LoadWaitingLearnersByCourseAsync(int courseId)
        {
            WaitingLearners = await _learningService.GetWaitingLearnersByCourseAsync(courseId);
        }

        private async Task LoadScheduleSlotsAsync()
        {
            var slots = await _scheduleSlotService.GetAllSlots();
            ScheduleSlots = slots.OrderBy(s => s.StartTime).ToList();
        }



        /// <summary>
        /// Lấy class CSS cho badge trạng thái học
        /// </summary>
        public static string GetStatusBadgeClass(string? status)
        {
            return status switch
            {
                "Đang học" => "bg-success",
                "Bảo lưu" => "bg-warning text-dark",
                "Học lại" => "bg-danger",
                "Hoàn thành" => "bg-primary",
                "Chưa bắt đầu" => "bg-secondary",
                _ => "bg-light text-dark"
            };
        }

        // AJAX endpoints removed - using MVC pattern with page reload instead
    }
}
