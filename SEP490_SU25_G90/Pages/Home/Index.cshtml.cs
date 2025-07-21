using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;

namespace SEP490_SU25_G90.Pages
{
    [Authorize(Policy = "GuestOrLearnerPolicy")]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsService _newsService;

        public IndexModel(ILogger<IndexModel> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public NewsListInformationResponse? Introduction { get; set; }
        
        [BindProperty]
        public IList<NewsListInformationResponse> BlogPosts { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Introduction = await _newsService.GetNewsByIdAsync(15);
            BlogPosts = await _newsService.GetTopNewsAsync();

            return Page();
        }
    }
}
