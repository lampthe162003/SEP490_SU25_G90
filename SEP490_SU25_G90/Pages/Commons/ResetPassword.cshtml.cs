using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ResetPasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự")]
        public string NewPassword { get; set; } = default!;

        [BindProperty]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu phải trùng với mật khẩu mới")]
        public string ConfirmPassword { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string? email = TempData["ResetEmail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                return Redirect("/ForgetPassword");
            }

            await _userService.ResetPasswordAsync(email, NewPassword);

            return Redirect("/Login");
        }
    }
}
