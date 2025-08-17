using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class UpdateLearnerModel : PageModel
    {
        private readonly IUserService _userService;

        public UpdateLearnerModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UpdateLearnerRequest UpdateRequest { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? MessageType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToPage("./ListLearningProfile");
            }

            try
            {
                var learner = await _userService.GetLearnerById(id.Value);
                if (learner == null)
                {
                    Message = "Không tìm thấy học viên!";
                    MessageType = "error";
                    return RedirectToPage("./ListLearningProfile");
                }

                // Map learner data to update request
                UpdateRequest = new UpdateLearnerRequest
                {
                    UserId = learner.UserId,
                    Email = learner.Email,
                    FirstName = learner.FirstName,
                    MiddleName = learner.MiddleName,
                    LastName = learner.LastName,
                    Dob = learner.Dob,
                    Gender = learner.Gender,
                    Phone = learner.Phone,
                    ProfileImageUrl = learner.ProfileImageUrl,
                    CccdNumber = learner.CccdNumber,
                    CccdImageFront = learner.CccdImageFront,
                    CccdImageBack = learner.CccdImageBack,
                    HealthCertificateImageUrl = learner.HealthCertificateImageUrl
                };

                return Page();
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tải thông tin học viên: {ex.Message}";
                MessageType = "error";
                return RedirectToPage("./ListLearningProfile");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var learner = await _userService.GetLearnerById(UpdateRequest.UserId);
                if (learner == null)
                {
                    Message = "Không tìm thấy học viên!";
                    MessageType = "error";
                    return RedirectToPage("./ListLearningProfile");
                }

                // Update learner information
                await _userService.UpdateLearnerInfoAsync(UpdateRequest.UserId, UpdateRequest);

                Message = "Cập nhật thông tin học viên thành công!";
                MessageType = "success";
                return RedirectToPage("./ListLearningProfile");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi cập nhật thông tin học viên: {ex.Message}";
                MessageType = "error";
                return Page();
            }
        }
    }
} 