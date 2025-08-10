using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.HumanResources.LearningProfile
{
    [Authorize(Roles = "human resources")]
    public class LearnerDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public LearnerDetailModel(IUserService userService)
        {
            _userService = userService;
        }

        public LearnerDetailResponse? Learner { get; set; }
        
        [TempData]
        public string? Message { get; set; }
        
        [TempData]
        public string? MessageType { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                Message = "ID học viên không hợp lệ!";
                MessageType = "error";
                return RedirectToPage("./ListLearningProfile");
            }

            Learner = await _userService.GetLearnerById(id);

            if (Learner == null)
            {
                Message = "Không tìm thấy thông tin học viên!";
                MessageType = "error";
                return RedirectToPage("./ListLearningProfile");
            }

            return Page();
        }
    }
} 