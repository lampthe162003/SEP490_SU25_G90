using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Classes
{
    [Authorize(Roles = "academic affairs")]
    public class CreateClassesModel : PageModel
    {
        private readonly Sep490Su25G90DbContext _context;
        private readonly IInstructorService _instructorService;
        private readonly ILearningApplicationService _learningService;

        public CreateClassesModel(
            Sep490Su25G90DbContext context,
            IInstructorService instructorService,
            ILearningApplicationService learningService)
        {
            _context = context;
            _instructorService = instructorService;
            _learningService = learningService;
        }

        [BindProperty]
        public string? ClassName { get; set; }

        [BindProperty]
        public int? InstructorId { get; set; }

        [BindProperty]
        public int? SelectedCourseId { get; set; }

        [BindProperty]
        public int MaxStudents { get; set; } = 10;

        [BindProperty]
        public List<int> SelectedLearnerIds { get; set; } = new();

        public List<SelectListItem> Courses { get; set; } = new();
        public List<SelectListItem> Instructors { get; set; } = new();

        public List<WaitingLearnerVm> WaitingLearners { get; set; } = new();

        public class WaitingLearnerVm
        {
            public int LearningId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Cccd { get; set; } = string.Empty;
            public string Status { get; set; } = "Chưa học";
            public string ProfileImageUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/1144/1144760.png";
            public bool Selected { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public int? CourseId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDropdownsAsync();
            await LoadWaitingLearnersAsync();

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

            // Luôn generate tên lớp khi vào trang
            if (SelectedCourseId.HasValue)
            {
                ClassName = await GenerateClassNameInternalAsync(SelectedCourseId.Value);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadDropdownsAsync();
            await LoadWaitingLearnersAsync();

            if (!SelectedCourseId.HasValue)
            {
                ModelState.AddModelError(nameof(SelectedCourseId), "Vui lòng chọn khóa học");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Bỏ qua input từ client, luôn generate tên lớp theo quy tắc để chống chỉnh sửa
                if (!SelectedCourseId.HasValue)
                {
                    ModelState.AddModelError(nameof(SelectedCourseId), "Vui lòng chọn khóa học");
                    return Page();
                }
                ClassName = await GenerateClassNameInternalAsync(SelectedCourseId.Value);

                var newClass = new vn.edu.fpt.Models.Class
                {
                    ClassName = ClassName,
                    InstructorId = InstructorId,
                    CourseId = SelectedCourseId
                };
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();

                // Limit selected learners by MaxStudents
                var learnerIds = SelectedLearnerIds.Take(Math.Max(0, MaxStudents)).ToList();
                foreach (var learningId in learnerIds)
                {
                    _context.ClassMembers.Add(new ClassMember
                    {
                        ClassId = newClass.ClassId,
                        LearnerId = learningId
                    });

                    // Tự động cập nhật trạng thái học thành "Đang học" nếu có giảng viên
                    if (InstructorId.HasValue)
                    {
                        var learningApp = await _context.LearningApplications
                            .FirstOrDefaultAsync(la => la.LearningId == learningId);
                        
                        if (learningApp != null && learningApp.LearningStatus != 1)
                        {
                            learningApp.LearningStatus = 1; // Đang học
                            Console.WriteLine($" [DEBUG] Cập nhật trạng thái học viên {learningId} thành 'Đang học'");
                        }
                    }
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Tạo lớp học thành công";
                return RedirectToPage("/AcademicAffairs/Classes/ClassDetails", new { id = newClass.ClassId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo lớp: " + ex.Message);
                return Page();
            }
        }

        public async Task<JsonResult> OnGetGenerateNameAsync(int courseId)
        {
            var name = await GenerateClassNameInternalAsync(courseId);
            return new JsonResult(new { name });
        }

        private async Task<string> GenerateClassNameInternalAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            var prefix = course?.CourseName ?? ($"KH-{courseId}");

            var existingNames = await _context.Classes
                .Where(c => c.CourseId == courseId && c.ClassName != null)
                .Select(c => c.ClassName!)
                .ToListAsync();

            var maxIndex = 0;
            foreach (var name in existingNames)
            {
                var marker = "-L";
                var idx = name.LastIndexOf(marker, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0 && idx + marker.Length < name.Length)
                {
                    var numStr = name[(idx + marker.Length)..];
                    if (int.TryParse(numStr, out var n) && n > maxIndex)
                    {
                        maxIndex = n;
                    }
                }
            }

            return $"{prefix}-L{maxIndex + 1}";
        }

        private async Task LoadDropdownsAsync()
        {
            Courses = await _context.Courses
                .OrderByDescending(c => c.CourseId)
                .Select(c => new SelectListItem
                {
                    Value = c.CourseId.ToString(),
                    Text = c.CourseName ?? ("KH-" + c.CourseId)
                })
                .ToListAsync();

            var instructorDtos = _instructorService.GetAllInstructors();
            Instructors = instructorDtos
                .Select(i => new SelectListItem
                {
                    Value = i.UserId.ToString(),
                    Text = string.Join(" ", new[] { i.FirstName, i.MiddleName, i.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)))
                }).ToList();
        }

        private async Task LoadWaitingLearnersAsync()
        {
            // Learners who are not assigned to any class yet or status not completed
            var learners = await _learningService.GetAllAsync();

            var waiting = learners
                .Where(x => x.LearningStatus != 4) // exclude completed
                .Select(x => new WaitingLearnerVm
                {
                    LearningId = x.LearningId,
                    FullName = x.LearnerFullName ?? "",
                    Cccd = x.LearnerCccdNumber ?? "",
                    Status = x.LearningStatusName ?? "",
                    ProfileImageUrl = string.IsNullOrWhiteSpace(x.LearnerCccdImageUrl) ?
                        "https://cdn-icons-png.flaticon.com/512/1144/1144760.png" :
                        x.LearnerCccdImageUrl.Split('|').FirstOrDefault() ?? "https://cdn-icons-png.flaticon.com/512/1144/1144760.png"
                })
                .ToList();

            WaitingLearners = waiting;
        }
    }
}
