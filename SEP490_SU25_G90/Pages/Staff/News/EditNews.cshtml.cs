using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;

namespace SEP490_SU25_G90.Pages.Staff.News
{
    [Authorize(Roles = "staff")]
    public class EditNewsModel : PageModel
    {
        private readonly INewsService _newsService;

        public EditNewsModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        [BindProperty]
        public NewsFormRequest Edit { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var result = await _newsService.GetNewsFormByIdAsync(id);
            if (result == null) return NotFound();
            Edit = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var success = await _newsService.EditNewsAsync(Edit);
            if (!success)
            {
                TempData["ErrorMessage"] = "Không tìm thấy bài viết cần sửa.";
                return RedirectToPage("/Admins/News/ListNews");
            }

            TempData["SuccessMessage"] = "Cập nhật thành công!";
            return RedirectToPage("/Admins/News/ListNews");
        }
    }
}
