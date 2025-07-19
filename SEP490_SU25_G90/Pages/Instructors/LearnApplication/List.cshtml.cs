using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.Pages.Instructors.LearnApplication
{
    [Authorize(Roles = "instructor")]
    public class ListLearningApplicationsModel : PageModel
    {
        private readonly IInstructorService _instructorService;
        private readonly Sep490Su25G90DbContext _context; // Inject DbContext để truy UserId

        public ListLearningApplicationsModel(
            IInstructorService instructorService,
            Sep490Su25G90DbContext context)
        {
            _instructorService = instructorService;
            _context = context;
        }

        public List<LearningApplicationsResponse> LearningApplications { get; set; } = new();

        // Fix: Replace 'Username' with 'Email' or another appropriate property that exists in the 'User' class.

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy tên người dùng hiện tại
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized();
            }

            // Truy vấn lấy instructorId từ email
            var instructor = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == username); 

            if (instructor == null)
            {
                return Unauthorized();
            }

            var instructorId = instructor.UserId;

            // Lấy danh sách hồ sơ học của học viên trong lớp của giảng viên này
            LearningApplications = await _instructorService.GetLearningApplicationsByInstructorAsync(instructorId);
            return Page();
        }
    }
}
