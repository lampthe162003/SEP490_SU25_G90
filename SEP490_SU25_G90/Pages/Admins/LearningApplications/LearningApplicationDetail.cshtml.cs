using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;

namespace SEP490_SU25_G90.Pages.Admins.LearningApplications
{
    [Authorize(Roles = "admin")]
    public class LearningApplicationDetailModel : PageModel
    {
        private readonly ILearningApplicationService _learningApplicationService;

        public LearningApplicationDetailModel(ILearningApplicationService learningApplicationService)
        {
            _learningApplicationService = learningApplicationService;
        }

        public LearningApplicationsResponse? LearningApplication { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Id { get; set; }

        [BindProperty]
        public byte NewStatus { get; set; }

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detail = await _learningApplicationService.GetDetailAsync(id.Value);
            if (detail == null)
            {
                return NotFound();
            }

            LearningApplication = detail;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            if (Id == null)
            {
                return NotFound();
            }

            if (NewStatus < 1 || NewStatus > 4)
            {
                StatusMessage = "⚠️ Trạng thái không hợp lệ.";
                return RedirectToPage(new { id = Id });
            }

            var result = await _learningApplicationService.UpdateStatusAsync(Id.Value, NewStatus);

            StatusMessage = result
                ? "✅ Cập nhật trạng thái thành công."
                : "❌ Cập nhật trạng thái thất bại.";

            return RedirectToPage(new { id = Id });
        }
    }
}
