using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService;

namespace SEP490_SU25_G90.Pages.Admins.LearningProfile
{
    [Authorize(Roles = "admin")]
    public class ManagerInstructorModel : PageModel
    {
        private readonly IInstructorService _instructorService;

        public ManagerInstructorModel(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        public IList<InstructorListInformationResponse> Instructors { get; set; } = new List<InstructorListInformationResponse>();
        
        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public byte? SearchLicenceType { get; set; }
        
        public List<SelectListItem> LicenceTypes { get; set; } = new List<SelectListItem>();
        
        [TempData]
        public string? Message { get; set; }
        
        [TempData]
        public string? MessageType { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPostSearch()
        {
            LoadData();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                _instructorService.DeleteInstructor(id);
                Message = "Xóa giảng viên thành công!";
                MessageType = "success";
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi xóa giảng viên: {ex.Message}";
                MessageType = "error";
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostAddSpecialization(int instructorId, byte licenceTypeId)
        {
            try
            {
                _instructorService.AddSpecialization(instructorId, licenceTypeId);
                Message = "Thêm chuyên môn thành công!";
                MessageType = "success";
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi thêm chuyên môn: {ex.Message}";
                MessageType = "error";
            }
            
            return RedirectToPage();
        }

        public IActionResult OnPostRemoveSpecialization(int instructorId, byte licenceTypeId)
        {
            try
            {
                _instructorService.RemoveSpecialization(instructorId, licenceTypeId);
                Message = "Xóa chuyên môn thành công!";
                MessageType = "success";
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi xóa chuyên môn: {ex.Message}";
                MessageType = "error";
            }
            
            return RedirectToPage();
        }

        private void LoadData()
        {
            // Load instructors with search filters
            Instructors = _instructorService.GetAllInstructors(SearchName, SearchLicenceType);
            
            // Load licence types for dropdown
            var licenceTypes = _instructorService.GetAllLicenceTypes();
            LicenceTypes = licenceTypes.Select(lt => new SelectListItem
            {
                Value = lt.LicenceTypeId.ToString(),
                Text = lt.LicenceCode
            }).ToList();
            
            // Add "All" option
            LicenceTypes.Insert(0, new SelectListItem { Value = "", Text = "Tất cả loại bằng" });
        }
    }
}
