using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class CreateLearnerModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateLearnerModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public CreateLearnerRequest CreateRequest { get; set; } = new();
        
        [TempData]
        public string? Message { get; set; }
        
        [TempData]
        public string? MessageType { get; set; }

        public void OnGet()
        {
            // Initialize page
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Check if email already exists
                var existingUserWithEmail = await _userService.DoesUserWithEmailExist(CreateRequest.Email!);
                if (existingUserWithEmail)
                {
                    ModelState.AddModelError("CreateRequest.Email", "Email này đã được sử dụng");
                    return Page();
                }

                // Check if CCCD number already exists
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber))
                {
                    var existingUserWithCccd = await _userService.DoesUserWithCccdExist(CreateRequest.CccdNumber);
                    if (existingUserWithCccd)
                    {
                        ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD này đã được sử dụng");
                        return Page();
                    }
                }

                // Check if phone number already exists
                if (!string.IsNullOrEmpty(CreateRequest.Phone))
                {
                    var existingUserWithPhone = await _userService.DoesUserWithPhoneExist(CreateRequest.Phone);
                    if (existingUserWithPhone)
                    {
                        ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại này đã được sử dụng");
                        return Page();
                    }
                }

                // Check if age is lower than 18
                if (CreateRequest.Dob > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
                    return Page();
                }

                // Check if age is too high (over 60 for learners)
                if (CreateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-60)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên không được quá 60.");
                    return Page();
                }

                // Validate CCCD number format
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.CccdNumber, @"^\d{12}$"))
                {
                    ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD phải có đúng 12 chữ số và chỉ chứa số");
                    return Page();
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(CreateRequest.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.Phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 số, bắt đầu bằng 03, 05, 07, 08, 09)");
                    return Page();
                }

                var password = await _userService.CreateLearnerAsync(CreateRequest);
                Message = $"Tạo tài khoản học viên thành công! Mật khẩu đã được gửi về email {CreateRequest.Email}";
                MessageType = "success";
                
                return RedirectToPage("./ListLearningProfile");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tạo tài khoản học viên: {ex.Message}";
                MessageType = "error";
                return Page();
            }
        }
    }
} 