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

                // Check if CCCD number already exists
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber))
                {
                    if (existingInstructors.Any(i => i.CccdNumber == CreateRequest.CccdNumber))
                    {
                        ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD này đã được sử dụng");
                        LoadAvailableLicenceTypes();
                        return Page();
                    }
                }

                // Check if phone number already exists
                if (!string.IsNullOrEmpty(CreateRequest.Phone))
                {
                    if (existingInstructors.Any(i => i.Phone == CreateRequest.Phone))
                    {
                        ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại này đã được sử dụng");
                        LoadAvailableLicenceTypes();
                        return Page();
                    }
                }

                // Check if age is lower than 18
                if (CreateRequest.Dob > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của giảng viên chưa đủ 18.");
                    LoadAvailableLicenceTypes();
                    return Page();
                }

                // Check if age is too high (over 65)
                if (CreateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-65)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của giảng viên không được quá 65.");
                    LoadAvailableLicenceTypes();
                    return Page();
                }

                // Validate CCCD number format
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.CccdNumber, @"^\d{12}$"))
                {
                    ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD phải có đúng 12 chữ số và chỉ chứa số");
                    LoadAvailableLicenceTypes();
                    return Page();
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(CreateRequest.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.Phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 số, bắt đầu bằng 03, 05, 07, 08, 09)");
                    LoadAvailableLicenceTypes();
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