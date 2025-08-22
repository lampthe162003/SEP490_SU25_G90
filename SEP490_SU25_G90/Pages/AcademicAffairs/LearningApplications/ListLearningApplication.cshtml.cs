using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.LearningApplications
{
    [Authorize(Roles = "academic affairs")]
    public class ListLearningApplicationsModel : PageModel
    {
        private readonly ILearningApplicationService _learningApplicationService;
        private readonly Sep490Su25G90DbContext _context;

        public ListLearningApplicationsModel(ILearningApplicationService learningApplicationService, Sep490Su25G90DbContext context)
        {
            _learningApplicationService = learningApplicationService;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        // Phân trang
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;

        // Bộ lọc trạng thái
        [BindProperty(SupportsGet = true)]
        public int? StatusFilter { get; set; }
        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int? LicenceTypeFilter { get; set; }
        public List<SelectListItem> LicenceTypeOptions { get; set; } = new();
        public async Task OnGetAsync()
        {
            // Load danh sách loại bằng
            LicenceTypeOptions = await _context.LicenceTypes
                .Select(l => new SelectListItem
                {
                    Value = l.LicenceTypeId.ToString(),
                    Text = l.LicenceCode
                })
                .ToListAsync();

            LicenceTypeOptions.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Tất cả loại bằng --"
            });
            var allApplications = await _learningApplicationService.GetAllAsync(SearchString, StatusFilter);

            int totalItems = allApplications.Count;
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            LearningApplications = allApplications
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }


    }
}