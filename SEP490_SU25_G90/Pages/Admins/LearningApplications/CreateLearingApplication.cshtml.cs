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
    [Authorize(Roles = "academic affairs")]
    public class CreateLearingApplicationModel : PageModel
    {
        private readonly Sep490Su25G90DbContext _context;
        private readonly ILearningApplicationService _learningApplicationService;

        public CreateLearingApplicationModel(
            Sep490Su25G90DbContext context,
            ILearningApplicationService learningApplicationService,
            IInstructorService instructorService)
        {
            _context = context;
            _learningApplicationService = learningApplicationService;
        }

        [BindProperty]
        public string SearchCccd { get; set; } = string.Empty;

        [BindProperty]
        public LearningApplicationsResponse? LearnerInfo { get; set; }

        public string? ErrorMessage { get; set; }
        public bool ShowForm { get; set; } = false;
        public bool IsEligibleToCreate { get; set; } = false;


        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");
            // Gán ngày hôm nay mặc định khi chưa tìm học viên
            if (LearnerInfo == null)
            {
                LearnerInfo = new LearningApplicationsResponse
                {
                    SubmittedAt = DateTime.Today
                };
            }
            return Page();
        }


        public async Task<IActionResult> OnPostSearchAsync()
        {
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");

            if (string.IsNullOrWhiteSpace(SearchCccd))
            {
                ErrorMessage = "Vui lòng nhập số CCCD.";
                ShowForm = false;
                return Page();
            }

            var learner = await _learningApplicationService.FindLearnerByCccdAsync(SearchCccd);
            if (learner == null)
            {
                ErrorMessage = "Không tìm thấy học viên với số CCCD này.";
                ShowForm = true;
                LearnerInfo = null;
                IsEligibleToCreate = false;
                return Page();
            }

            // Luôn hiển thị thông tin học viên
            LearnerInfo = learner;
            LearnerInfo.SubmittedAt ??= DateTime.Today;
            ShowForm = true;

            // Nếu học viên đã có hồ sơ không hợp lệ
            if (learner.LearningStatusName == "Đang học" || learner.LearningStatusName == "Hoàn thành")
            {
                ErrorMessage = $"Học viên đã có hồ sơ với trạng thái: {learner.LearningStatusName}. Không thể tạo mới.";
                IsEligibleToCreate = false;
                return Page();
            }

            // Cho phép tạo mới
            IsEligibleToCreate = true;
            return Page();


            //LearnerInfo = learner;
            //LearnerInfo.SubmittedAt ??= DateTime.Today; // Nếu chưa có thì gán mặc định
            //ShowForm = true;
            //return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "LicenceTypeId", "LicenceCode");

            if (LearnerInfo == null)
            {
                ErrorMessage = "Vui lòng tìm kiếm học viên trước.";
                ShowForm = false;
                return Page();
            }

            // Gán ngày đăng ký mặc định nếu chưa có
            if (!LearnerInfo.SubmittedAt.HasValue)
            {
                LearnerInfo.SubmittedAt = DateTime.Today;
            }

            // Kiểm tra ngày sinh < ngày đăng ký
            if (LearnerInfo.LearnerDob != null && LearnerInfo.SubmittedAt != null &&
                LearnerInfo.LearnerDob >= LearnerInfo.SubmittedAt)
            {
                ModelState.AddModelError("LearnerInfo.LearnerDob", "Ngày sinh phải trước ngày đăng ký.");
            }

            // Kiểm tra lỗi model state
            if (!ModelState.IsValid)
            {
                foreach (var kv in ModelState)
                {
                    foreach (var err in kv.Value.Errors)
                    {
                        Console.WriteLine($"[ModelState ERROR] {kv.Key}: {err.ErrorMessage}");
                    }
                }
                ShowForm = true;
                return Page();
            }

            // Tạo entity và lưu
            var entity = new LearningApplication
            {
                LearnerId = LearnerInfo.LearnerId ?? 0,
                LicenceTypeId = LearnerInfo.LicenceTypeId,
                SubmittedAt = LearnerInfo.SubmittedAt,
                TheoryScore = 0,
                SimulationScore = 0,
                ObstacleScore = 0,
                PracticalScore = 0,
                LearningStatus = 0,
                TestEligibility = false
            };

            await _learningApplicationService.AddAsync(entity);
            TempData["SuccessMessage"] = $"Đã tạo hồ sơ thành công cho học viên {LearnerInfo?.LearnerFullName}";
            return RedirectToPage("./ListLearningApplication");
        }

    }
}
