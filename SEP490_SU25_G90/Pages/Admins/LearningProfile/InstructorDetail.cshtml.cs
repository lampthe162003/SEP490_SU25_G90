using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.Admins.LearningProfile
{
    public class InstructorDetailModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        public InstructorDetailModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public InstructorListInformationResponse? Instructor { get; set; }
        
        [TempData]
        public string? Message { get; set; }
        
        [TempData]
        public string? MessageType { get; set; }

        public IActionResult OnGet(int id)
        {
            if (id <= 0)
            {
                Message = "ID giảng viên không hợp lệ!";
                MessageType = "error";
                return RedirectToPage("./ManagerInstructor");
            }

            Instructor = _instructorService.GetInstructorById(id);

            if (Instructor == null)
            {
                Message = "Không tìm thấy thông tin giảng viên!";
                MessageType = "error";
                return RedirectToPage("./ManagerInstructor");
            }

            return Page();
        }
    }
}
