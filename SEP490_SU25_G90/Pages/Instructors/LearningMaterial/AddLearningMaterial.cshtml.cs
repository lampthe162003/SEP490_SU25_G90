using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.LearningMaterial
{
    [Authorize(Roles = "instructor")]
    public class AddLearningMaterialModel : PageModel
    {
        private readonly ILearningMaterialService _learningMaterialService;

        public AddLearningMaterialModel(ILearningMaterialService learningMaterialService)
        {
            _learningMaterialService = learningMaterialService;
        }

        [BindProperty]
        public LearningMaterialFormRequest Input { get; set; } = new();

        public List<SelectListItem> LicenceTypeOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var licenceTypes = await _learningMaterialService.GetLicenceTypesAsync();
            LicenceTypeOptions = licenceTypes.Select(x => new SelectListItem
            {
                Value = x.LicenceTypeId.ToString(),
                Text = x.LicenceCode
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _learningMaterialService.AddMaterialAsync(Input);
            return RedirectToPage("/Instructors/LearningMaterial/LearningMaterialList");
        }
    }
}
