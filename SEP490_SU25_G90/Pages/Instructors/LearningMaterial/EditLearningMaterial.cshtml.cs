using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
    public class EditLearningMaterialModel : PageModel
    {
        private readonly ILearningMaterialService _learningMaterialService;

        public EditLearningMaterialModel(ILearningMaterialService learningMaterialService)
        {
            _learningMaterialService = learningMaterialService;
        }

        [BindProperty]
        public LearningMaterialFormRequest Input { get; set; } = new();
        public List<SelectListItem> LicenceTypeOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var form = await _learningMaterialService.GetFormByIdAsync(id);
            if (form == null) return NotFound();

            Input = form;

            var licenceTypes = await _learningMaterialService.GetLicenceTypesAsync();
            LicenceTypeOptions = licenceTypes.Select(x => new SelectListItem
            {
                Value = x.LicenceTypeId.ToString(),
                Text = x.LicenceCode
            }).ToList();

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var success = await _learningMaterialService.EditMaterialAsync(Input);
            if (!success) return NotFound();

            return RedirectToPage("/Instructors/LearningMaterial/LearningMaterialList");
        }
    }
}
