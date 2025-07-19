using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Learner.LearningMaterial
{
    [Authorize(Policy = "GuestOrLearnerPolicy")]
    public class LearningMaterialListModel : PageModel
    {
        private readonly ILearningMaterialService _iLearningMaterialService;

        public LearningMaterialListModel(ILearningMaterialService iLearningMaterialService)
        {
            _iLearningMaterialService = iLearningMaterialService;
        }

        public List<LearningMaterialListInformationResponse> Materials { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            const int pageSize = 6;

            var (items, totalCount) = await _iLearningMaterialService.GetPagedMaterialsAsync(CurrentPage, pageSize);
            Materials = items;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
