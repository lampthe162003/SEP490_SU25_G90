using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;

namespace SEP490_SU25_G90.Pages
{
    [Authorize(Policy = "GuestOrLearnerPolicy")]
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        [BindProperty]
        public IList<NewsListInformationResponse> BlogPosts { get; set; } = default!;
        
        public async Task<IActionResult> OnGetAsync()
        {
            BlogPosts = await _newsService.GetTopNewsAsync();

            return Page();
        }
    }
}
