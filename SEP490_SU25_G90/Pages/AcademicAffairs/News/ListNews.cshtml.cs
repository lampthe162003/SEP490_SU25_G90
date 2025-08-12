using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.News
{
    [Authorize(Roles = "academic affairs")]
    public class ListNewsModel : PageModel
    {
        private readonly INewsService _iNewsService;

        public ListNewsModel(INewsService iNewsService)
        {
            _iNewsService = iNewsService;
        }

        public IList<NewsListInformationResponse> News { get; set; } = new List<NewsListInformationResponse>();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            const int pageSize = 6;
            var (items, totalItems) = await _iNewsService.GetPagedNewsAsync(CurrentPage, pageSize);

            News = items;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, string imagePath)
        {
            var success = await _iNewsService.DeleteNewsAsync(id);

            if (!success)
            {           
                TempData["SuccessMessage"] = "Xóa tin tức thất bại hoặc không tìm thấy.";
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa tin tức thành công.";
            }

            return RedirectToPage();
        }
    }
}
