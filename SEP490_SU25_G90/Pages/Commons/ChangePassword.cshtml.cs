using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Versioning;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class ChangePasswordModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string CurrentPassword { get; set; } = default!;

        [BindProperty]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string NewPassword { get; set; } = default!;

        [BindProperty]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu phải trùng khớp với mật khẩu mới.")]
        public string PasswordVerification { get; set; } = default!;

        private readonly IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userEmail = User.Identity?.Name;
            var userId = User.FindFirst("user_id")?.Value;

            if (userEmail == null || userId == null)
            {
                return Redirect("Error403");
            }
            var user = _userService.GetLoginDetails(userEmail, CurrentPassword);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không đúng. Vui lòng thử lại");
                return Page();
            }

            await _userService.UpdatePasswordAsync(int.Parse(userId), NewPassword);

            if (User.IsInRole("admin"))
            {
                return Redirect("/Admin/Dashboard");
            }
            else if (User.IsInRole("instructor"))
            {
                return Redirect("Instructor/LearningMaterial/List");
            }
            return Redirect("/Home/Index");
        }
    }
}
