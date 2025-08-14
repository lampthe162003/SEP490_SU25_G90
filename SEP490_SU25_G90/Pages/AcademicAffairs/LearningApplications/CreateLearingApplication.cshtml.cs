using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.LearningApplications
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

        // Helper method to calculate age
        private int CalculateAge(DateTime birthDate, DateTime registrationDate)
        {
            int age = registrationDate.Year - birthDate.Year;
            if (registrationDate.Month < birthDate.Month || 
                (registrationDate.Month == birthDate.Month && registrationDate.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }

        // Helper method to validate age requirements
        private bool ValidateAgeRequirement(DateTime? birthDate, DateTime? registrationDate, string? licenceCode)
        {
            if (birthDate == null || registrationDate == null || string.IsNullOrEmpty(licenceCode))
                return false;

            int age = CalculateAge(birthDate.Value, registrationDate.Value);

            return licenceCode switch
            {
                "B1" or "B2" => age >= 18,
                "C" => age >= 21,
                _ => true // Other license types don't have age restrictions
            };
        }

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

            // Validate CCCD format - must be exactly 12 digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(SearchCccd, @"^\d{12}$"))
            {
                ErrorMessage = "Số CCCD phải có đúng 12 chữ số.";
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

            // Kiểm tra điều kiện tuổi theo loại bằng
            if (LearnerInfo.LearnerDob != null && LearnerInfo.SubmittedAt != null && LearnerInfo.LicenceTypeId.HasValue)
            {
                // Lấy thông tin loại bằng
                var licenceType = await _context.LicenceTypes.FindAsync(LearnerInfo.LicenceTypeId.Value);
                if (licenceType != null)
                {
                    bool isValidAge = ValidateAgeRequirement(LearnerInfo.LearnerDob, LearnerInfo.SubmittedAt, licenceType.LicenceCode);
                    
                    if (!isValidAge)
                    {
                        int currentAge = CalculateAge(LearnerInfo.LearnerDob.Value, LearnerInfo.SubmittedAt.Value);
                        string requiredAge = licenceType.LicenceCode switch
                        {
                            "B1" or "B2" => "18",
                            "C" => "21",
                            _ => "không xác định"
                        };
                        
                        ModelState.AddModelError("LearnerInfo.LicenceTypeId", 
                            $"Học viên phải đủ {requiredAge} tuổi để đăng ký bằng {licenceType.LicenceCode}. " +
                            $"Hiện tại học viên {currentAge} tuổi (tính đến ngày đăng ký).");
                    }
                }
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
