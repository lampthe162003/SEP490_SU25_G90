using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.Learner.LearningProfile
{
    [Authorize(Policy = "GuestOrLearnerPolicy")]
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

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            var userIdClaim = User.FindFirst("user_id")?.Value;

            Learner = await _userService.GetLearnerById(int.Parse(userIdClaim));

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