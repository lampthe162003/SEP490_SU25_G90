using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.LearningApplications
{
    [Authorize(Roles = "admin")]
    public class CreateLearingApplicationModel : PageModel
    {
        private readonly Sep490Su25G90DbContext _context;
        private readonly ILearningApplicationService _learningApplicationService;
        private readonly IInstructorService _instructorService;

        public CreateLearingApplicationModel(
            Sep490Su25G90DbContext context,
            ILearningApplicationService learningApplicationService,
            IInstructorService instructorService)
        {
            _context = context;
            _learningApplicationService = learningApplicationService;
            _instructorService = instructorService;
        }

        [BindProperty]
        public string SearchCccd { get; set; } = string.Empty;

        [BindProperty]
        public LearningApplicationsResponse? LearnerInfo { get; set; }

        public List<InstructorListInformationResponse> Instructors { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public bool ShowForm { get; set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");            
            Instructors = _instructorService.GetAllInstructors().ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {            
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");
            Instructors = _instructorService.GetAllInstructors().ToList();
            if (string.IsNullOrWhiteSpace(SearchCccd))
            {
                ErrorMessage = "Vui lòng nhập số CCCD.";
                ShowForm = false;
                return Page();
            }

            // Tìm học viên theo CCCD
            var learner = await _learningApplicationService.FindLearnerByCccdAsync(SearchCccd);
            if (learner == null)
            {
                ErrorMessage = "Không tìm thấy học viên với số CCCD này.";
                ShowForm = false;
                return Page();
            }

            // Kiểm tra trạng thái hồ sơ
            if (learner.LearningStatusName == "Đang học" || learner.LearningStatusName == "Hoàn thành" || learner.LearningStatusName == "Đã huỷ")
            {
                ErrorMessage = $"Học viên đã có hồ sơ với trạng thái: {learner.LearningStatusName}. Không thể tạo mới.";
                ShowForm = false;
                return Page();
            }
            LearnerInfo = learner;
            ShowForm = true;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");
            Instructors = _instructorService.GetAllInstructors().ToList();
            if (LearnerInfo == null)
            {
                ErrorMessage = "Vui lòng tìm kiếm học viên trước.";
                ShowForm = false;
                return Page();
            }

            // Validate ngày sinh phải trước ngày đăng ký
            if (LearnerInfo.LearnerDob != null && LearnerInfo.SubmittedAt != null && LearnerInfo.LearnerDob >= LearnerInfo.SubmittedAt)
            {
                ModelState.AddModelError("LearnerInfo.LearnerDob", "Ngày sinh phải trước ngày đăng ký.");
                ShowForm = true;
                return Page();
            }
            if (!ModelState.IsValid)
            {
                ShowForm = true;
                return Page();
            }
            if (LearnerInfo.InstructorId == null)
            {
                ModelState.AddModelError("LearnerInfo.InstructorId", "Vui lòng chọn giảng viên.");
                ShowForm = true;
                return Page();
            }

            // Tạo hồ sơ học mới
            var entity = new LearningApplication
            {
                LearnerId = LearnerInfo.LearnerId ?? 0,
                LicenceTypeId = LearnerInfo.LicenceTypeId,
                SubmittedAt = LearnerInfo.SubmittedAt,
                //InstructorId = LearnerInfo.InstructorId
                // Điểm mặc định là 0 vì chưa thi
                TheoryScore = 0,
                SimulationScore = 0,
                ObstacleScore = 0,
                PracticalScore = 0,
                LearningStatus = 1
                // Các trường khác nếu cần
            };
            await _learningApplicationService.AddAsync(entity);
            return RedirectToPage("./ListLearningApplication");
        }
    }
}