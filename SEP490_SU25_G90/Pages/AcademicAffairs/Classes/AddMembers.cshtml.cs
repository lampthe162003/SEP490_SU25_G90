using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Classes
{
    [Authorize(Roles = "academic affairs")]
    public class AddMembersModel : PageModel
    {
        private readonly Sep490Su25G90DbContext _context;
        private readonly ILearningApplicationService _learningService;

        public AddMembersModel(Sep490Su25G90DbContext context, ILearningApplicationService learningService)
        {
            _context = context;
            _learningService = learningService;
        }

        [BindProperty(SupportsGet = true)]
        public int ClassId { get; set; }

        [BindProperty]
        public List<int> SelectedLearnerIds { get; set; } = new();

        public List<CandidateVm> Candidates { get; set; } = new();

        public class CandidateVm
        {
            public int LearningId { get; set; }
            public string FullName { get; set; } = string.Empty;
            public string Cccd { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string ProfileImageUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/1144/1144760.png";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var classExists = await _context.Classes.AnyAsync(c => c.ClassId == ClassId);
            if (!classExists) return NotFound();

            await LoadCandidatesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var classEntity = await _context.Classes
                .Include(c => c.Instructor)
                .Include(c => c.Course)
                .FirstOrDefaultAsync(c => c.ClassId == ClassId);
            if (classEntity == null) return NotFound();

            if (!SelectedLearnerIds.Any())
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn ít nhất một học viên");
                await LoadCandidatesAsync();
                return Page();
            }

            // Kiểm tra số lượng tối đa 5 học viên/lớp
            var currentMemberCount = await _context.ClassMembers.CountAsync(cm => cm.ClassId == ClassId);
            var selectedDistinctIds = SelectedLearnerIds.Distinct().ToList();
            var alreadyInClassIds = await _context.ClassMembers
                .Where(cm => cm.ClassId == ClassId && cm.LearnerId != null)
                .Select(cm => cm.LearnerId!.Value)
                .ToListAsync();
            var idsToAdd = selectedDistinctIds.Where(id => !alreadyInClassIds.Contains(id)).ToList();
            var remainingSlots = 5 - currentMemberCount;
            if (remainingSlots <= 0 || idsToAdd.Count > remainingSlots)
            {
                ModelState.AddModelError(string.Empty, $"Lớp chỉ cho phép tối đa 5 học viên. Còn trống {Math.Max(remainingSlots, 0)} chỗ.");
                await LoadCandidatesAsync();
                return Page();
            }

            // Xác thực loại bằng của học viên trùng với lớp (theo Course.LicenceTypeId)
            var classLicenceTypeId = classEntity.Course?.LicenceTypeId;
            if (classLicenceTypeId.HasValue)
            {
                var invalidLicenceLearners = await _context.LearningApplications
                    .Where(la => idsToAdd.Contains(la.LearningId))
                    .Where(la => la.LicenceTypeId != classLicenceTypeId)
                    .Select(la => la.LearningId)
                    .ToListAsync();
                if (invalidLicenceLearners.Any())
                {
                    ModelState.AddModelError(string.Empty, "Chỉ có thể thêm học viên có loại bằng phù hợp với lớp.");
                    await LoadCandidatesAsync();
                    return Page();
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var learningId in SelectedLearnerIds.Distinct())
                {
                    var exists = await _context.ClassMembers.AnyAsync(cm => cm.ClassId == ClassId && cm.LearnerId == learningId);
                    if (!exists)
                    {
                        _context.ClassMembers.Add(new ClassMember
                        {
                            ClassId = ClassId,
                            LearnerId = learningId
                        });

                        // Tự động cập nhật trạng thái học thành "Đang học" nếu có giảng viên
                        if (classEntity.InstructorId.HasValue)
                        {
                            var learningApp = await _context.LearningApplications
                                .FirstOrDefaultAsync(la => la.LearningId == learningId);

                            if (learningApp != null && learningApp.LearningStatus != 1)
                            {
                                learningApp.LearningStatus = 1; // Đang học
                                _context.LearningApplications.Update(learningApp);
                                Console.WriteLine($" [DEBUG] Cập nhật trạng thái học viên {learningId} thành 'Đang học'");
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["SuccessMessage"] = "Đã thêm học viên vào lớp";
                return RedirectToPage("/AcademicAffairs/Classes/ClassDetails", new { id = ClassId });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm học viên: " + ex.Message);
                await LoadCandidatesAsync();
                return Page();
            }
        }

        private async Task LoadCandidatesAsync()
        {
            var list = await _learningService.GetAllAsync();
            var alreadyMemberIds = await _context.ClassMembers
                .Where(cm => cm.ClassId == ClassId)
                .Select(cm => cm.LearnerId!.Value)
                .ToListAsync();
            // Lấy loại bằng của lớp qua Course
            var classLicenceTypeId = await _context.Classes
                .Include(c => c.Course)
                .Where(c => c.ClassId == ClassId)
                .Select(c => c.Course!.LicenceTypeId)
                .FirstOrDefaultAsync();

            Candidates = list
                .Where(x => x.LearningStatus != 4 && x.LearningStatus != 1 && !alreadyMemberIds.Contains(x.LearningId))
                .Where(x => !classLicenceTypeId.HasValue || x.LicenceTypeId == classLicenceTypeId)
                .Select(x => new CandidateVm
                {
                    LearningId = x.LearningId,
                    FullName = x.LearnerFullName ?? string.Empty,
                    Cccd = x.LearnerCccdNumber ?? string.Empty,
                    Status = x.LearningStatusName ?? string.Empty,
                    ProfileImageUrl = _context.Users.FirstOrDefault(z => z.UserId == x.LearnerId)?.ProfileImageUrl?.ToString() ??
                        "https://cdn-icons-png.flaticon.com/512/1144/1144760.png"
                })
                .ToList();
        }
    }
}

