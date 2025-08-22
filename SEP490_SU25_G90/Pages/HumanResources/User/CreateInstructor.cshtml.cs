using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class CreateInstructorModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        public CreateInstructorModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [BindProperty]
        public CreateInstructorRequest CreateRequest { get; set; } = new();

        public List<LicenceTypeResponse> AvailableLicenceTypes { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? MessageType { get; set; }

        public void OnGet()
        {
            LoadAvailableLicenceTypes();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                LoadAvailableLicenceTypes();
                return Page();
            }

            try
            {
                // Check if email already exists
                var existingInstructors = _instructorService.GetAllInstructors();
                if (existingInstructors.Any(i => i.Email == CreateRequest.Email))
                {
                    ModelState.AddModelError("CreateRequest.Email", "Email này đã được sử dụng");
                    LoadAvailableLicenceTypes();
                    return Page();
                }

                //Check if age is lower than 18
                if (CreateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
                    return Page();
                }

                await _instructorService.CreateInstructorAsync(CreateRequest);
                Message = $"Tạo tài khoản giảng viên thành công! Mật khẩu đã được gửi về email {CreateRequest.Email}";
                MessageType = "success";

                return RedirectToPage("./ManagerInstructor");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tạo tài khoản giảng viên: {ex.Message}";
                MessageType = "error";
                LoadAvailableLicenceTypes();
                return Page();
            }
        }

        private void LoadAvailableLicenceTypes()
        {
            AvailableLicenceTypes = _instructorService.GetAllLicenceTypes();
        }
    }
}