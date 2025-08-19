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

                //Check if age is lower than 18
                if (CreateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
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