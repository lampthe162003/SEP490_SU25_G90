using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.NewsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Admins.News
{
    [Authorize(Roles = "admin")]
    public class AddNewsModel : PageModel
    {
        private readonly INewsService _newsService;

        public AddNewsModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        [BindProperty]
        public NewsFormRequest Input { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (userIdClaim == null) return Unauthorized();

            int authorId = int.Parse(userIdClaim);
            await _newsService.AddNewsAsync(Input, authorId);

            return RedirectToPage("/Admins/News/ListNews");
        }
    }
}
