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
    public class LearningMaterialDetailsModel : PageModel
    {
        private readonly ILearningMaterialService _iLearningMaterialService;

        public LearningMaterialDetailsModel(ILearningMaterialService iLearningMaterialService)
        {
            _iLearningMaterialService = iLearningMaterialService;
        }

        public LearningMaterialListInformationResponse? Material { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Material = await _iLearningMaterialService.GetMaterialByIdAsync(id);
            if (Material == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
