using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class UpdateInstructorModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        public UpdateInstructorModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [BindProperty]
        public UpdateInstructorRequest UpdateRequest { get; set; } = new();

        public List<LicenceTypeResponse> AvailableLicenceTypes { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? MessageType { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToPage("./ManagerInstructor");
            }

            try
            {
                var instructor = _instructorService.GetInstructorById(id.Value);
                if (instructor == null)
                {
                    Message = "Không tìm thấy giảng viên!";
                    MessageType = "error";
                    return RedirectToPage("./ManagerInstructor");
                }

                // Map instructor data to update request
                UpdateRequest = new UpdateInstructorRequest
                {
                    UserId = instructor.UserId,
                    Email = instructor.Email,
                    FirstName = instructor.FirstName,
                    MiddleName = instructor.MiddleName,
                    LastName = instructor.LastName,
                    Dob = instructor.Dob,
                    Gender = instructor.Gender,
                    Phone = instructor.Phone,
                    ProfileImageUrl = instructor.ProfileImageUrl,
                    CccdNumber = instructor.CccdNumber,
                    CccdImageFront = instructor.CccdImageUrl, // ImageMt - mặt trước
                    CccdImageBack = instructor.CccdImageUrlMs, // ImageMs - mặt sau
                    SelectedSpecializations = instructor.Specializations.Select(s => s.LicenceTypeId).ToList()
                };

                LoadAvailableLicenceTypes();
                return Page();
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tải thông tin giảng viên: {ex.Message}";
                MessageType = "error";
                return RedirectToPage("./ManagerInstructor");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                LoadAvailableLicenceTypes();
                return Page();
            }

            try
            {
                var instructor = _instructorService.GetInstructorById(UpdateRequest.UserId);
                if (instructor == null)
                {
                    Message = "Không tìm thấy giảng viên!";
                    MessageType = "error";
                    return RedirectToPage("./ManagerInstructor");
                }

                // Update specializations first
                UpdateSpecializations(UpdateRequest.UserId, instructor.Specializations.Select(s => s.LicenceTypeId).ToList(), UpdateRequest.SelectedSpecializations);

                // Update instructor basic information using UpdateInstructorInfo method
                await _instructorService.UpdateInstructorInfoAsync(UpdateRequest.UserId, UpdateRequest);

                Message = "Cập nhật thông tin giảng viên thành công!";
                MessageType = "success";
                return RedirectToPage("./ManagerInstructor");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi cập nhật thông tin giảng viên: {ex.Message}";
                MessageType = "error";
                LoadAvailableLicenceTypes();
                return Page();
            }
        }

        private void LoadAvailableLicenceTypes()
        {
            AvailableLicenceTypes = _instructorService.GetAllLicenceTypes();
        }

        private void UpdateSpecializations(int instructorId, List<byte> currentSpecializations, List<byte> newSpecializations)
        {
            // Remove specializations that are no longer selected
            var specializationsToRemove = currentSpecializations.Except(newSpecializations).ToList();
            foreach (var licenceTypeId in specializationsToRemove)
            {
                _instructorService.RemoveSpecialization(instructorId, licenceTypeId);
            }

            // Add new specializations
            var specializationsToAdd = newSpecializations.Except(currentSpecializations).ToList();
            foreach (var licenceTypeId in specializationsToAdd)
            {
                _instructorService.AddSpecialization(instructorId, licenceTypeId);
            }
        }
    }
}