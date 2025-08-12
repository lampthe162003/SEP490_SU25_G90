using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.AcademicAffairs.Classes
{
    [Authorize(Roles = "academic affairs")]
    public class UpdateClassesModel : PageModel
    {
        private readonly Sep490Su25G90DbContext _context;
        private readonly IInstructorService _instructorService;

        public UpdateClassesModel(Sep490Su25G90DbContext context, IInstructorService instructorService)
        {
            _context = context;
            _instructorService = instructorService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public string? ClassName { get; set; }

        [BindProperty]
        public int? InstructorId { get; set; }

        public List<SelectListItem> Instructors { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var entity = await _context.Classes.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.ClassId == Id);
            if (entity == null) return NotFound();

            ClassName = entity.ClassName;
            InstructorId = entity.InstructorId;

            LoadInstructors();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var entity = await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == Id);
            if (entity == null) return NotFound();

            if (string.IsNullOrWhiteSpace(ClassName))
            {
                ModelState.AddModelError(nameof(ClassName), "Tên lớp không được để trống");
            }
            if (!ModelState.IsValid)
            {
                LoadInstructors();
                return Page();
            }

            entity.ClassName = ClassName; // giữ nguyên tên nếu theo quy tắc đã cố định, có thể cho readonly ở UI
            entity.InstructorId = InstructorId;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật lớp học thành công";
            return RedirectToPage("/AcademicAffairs/Classes/ClassDetails", new { id = Id });
        }

        private void LoadInstructors()
        {
            var list = _instructorService.GetAllInstructors();
            Instructors = list.Select(i => new SelectListItem
            {
                Value = i.UserId.ToString(),
                Text = string.Join(" ", new[] { i.FirstName, i.MiddleName, i.LastName }.Where(x => !string.IsNullOrWhiteSpace(x)))
            }).ToList();
        }
    }
}
