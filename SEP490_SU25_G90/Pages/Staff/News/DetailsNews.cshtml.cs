using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Staff.News
{
    [Authorize(Roles = "staff")]
    public class DetailsNewsModel : PageModel
    {
        private readonly INewsService _iNewsService;

        public DetailsNewsModel(INewsService iNewsService)
        {
            _iNewsService = iNewsService;
        }
        public NewsListInformationResponse News { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound(); 
            }

            var news = await _iNewsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            News = news;
            return Page();
        }
    }
}
