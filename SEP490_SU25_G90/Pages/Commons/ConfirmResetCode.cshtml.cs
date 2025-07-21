using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.Services.ResetCodeService;
using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class ConfirmResetCodeModel : PageModel
    {
        private readonly IResetCodeStorageService _storageService;

        public ConfirmResetCodeModel(IResetCodeStorageService storageService)
        {
            _storageService = storageService;
        }

        [BindProperty]
        public string? Message { get; set; }

        [BindProperty]
        public string Email { get; set; } = default!;

        [BindProperty]
        [Display(Name = "Mã xác nhận")]
        [Required(ErrorMessage = "Mã xác nhận không được để trống")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã xác nhận phải có 6 chữ số")]
        public string ResetCode { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string email)
        {
            Email = email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var storedResetCode = _storageService.GetCode(Email);

            if (storedResetCode == null)
            {
                ModelState.AddModelError(string.Empty, "Đã có lỗi xảy ra. Vui lòng thử lại sau.");
            }
            else if (storedResetCode.Value.ExpiresAt < DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Mã xác nhận đã hết hạn.");
            }
            else if (storedResetCode.Value.Code != ResetCode)
            {
                ModelState.AddModelError(string.Empty, "Mã xác nhận không đúng.");
            }

            _storageService.RemoveCode(Email);
            TempData["ResetEmail"] = Email;
            return Redirect("/ResetPassword");
        }
    }
}
