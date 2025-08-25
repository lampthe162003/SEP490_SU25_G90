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

                // Check if CCCD number already exists (exclude current user)
                if (!string.IsNullOrEmpty(UpdateRequest.CccdNumber))
                {
                    var existingUserWithCccd = await _userService.DoesUserWithCccdExistExcludingUser(UpdateRequest.CccdNumber, UpdateRequest.UserId);
                    if (existingUserWithCccd)
                    {
                        ModelState.AddModelError("UpdateRequest.CccdNumber", "Số CCCD này đã được sử dụng");
                        return Page();
                    }
                }

                // Check if phone number already exists (exclude current user)
                if (!string.IsNullOrEmpty(UpdateRequest.Phone))
                {
                    var existingUserWithPhone = await _userService.DoesUserWithPhoneExistExcludingUser(UpdateRequest.Phone, UpdateRequest.UserId);
                    if (existingUserWithPhone)
                    {
                        ModelState.AddModelError("UpdateRequest.Phone", "Số điện thoại này đã được sử dụng");
                        return Page();
                    }
                }

                // Check if age is valid
                if (UpdateRequest.Dob.HasValue)
                {
                    if (UpdateRequest.Dob > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                    {
                        ModelState.AddModelError("UpdateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
                        return Page();
                    }

                    if (UpdateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-60)))
                    {
                        ModelState.AddModelError("UpdateRequest.Dob", "Tuổi của học viên không được quá 60.");
                        return Page();
                    }
                }

                // Validate CCCD number format
                if (!string.IsNullOrEmpty(UpdateRequest.CccdNumber) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.CccdNumber, @"^\d{12}$"))
                {
                    ModelState.AddModelError("UpdateRequest.CccdNumber", "Số CCCD phải có đúng 12 chữ số và chỉ chứa số");
                    return Page();
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(UpdateRequest.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.Phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    ModelState.AddModelError("UpdateRequest.Phone", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 số, bắt đầu bằng 03, 05, 07, 08, 09)");
                    return Page();
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