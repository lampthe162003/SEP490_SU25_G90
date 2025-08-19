using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.Commons;
using SEP490_SU25_G90.vn.edu.fpt.Services.EmailService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ResetCodeService;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IResetCodeStorageService _storageService;
        private readonly IUserService _userService;

        public ForgetPasswordModel(IEmailService emailService, 
            IResetCodeStorageService storageService, 
            IUserService userService)
        {
            _emailService = emailService;
            _storageService = storageService;
            _userService = userService;
        }

        [BindProperty]
        public string? Message { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = default!;

        private TimeSpan resetCodeExpireSpan = TimeSpan.FromMinutes(10);

        public async Task<IActionResult> OnGetAsync(string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                Email = email;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool doesUserExist = await _userService.DoesUserWithEmailExist(Email);
            if (!doesUserExist)
            {
                Message = "Email không tồn tại. Vui lòng nhập lại.";
                return Page();
            }

            var resetCode = StringUtils.Generate6DigitCode();
            _storageService.SaveCode(Email, resetCode, resetCodeExpireSpan);
            await _emailService.SendResetCodeAsync(Email, resetCode);
            return Redirect($"/ConfirmResetCode?email={Email}");
        }
    }
}
