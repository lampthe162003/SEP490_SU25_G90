using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using SEP490_SU25_G90.vn.edu.fpt.Services.AddressService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class UpdateLearnerModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        public UpdateLearnerModel(IUserService userService, IAddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        [BindProperty]
        public UpdateLearnerRequest UpdateRequest { get; set; } = new();

        public List<CityResponse> AvailableCities { get; set; } = new();
        public List<ProvinceResponse> AvailableProvinces { get; set; } = new();
        public List<WardResponse> AvailableWards { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? MessageType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToPage("./ListLearningProfile");
            }

            try
            {
                var learner = await _userService.GetLearnerById(id.Value);
                if (learner == null)
                {
                    Message = "Không tìm thấy học viên!";
                    MessageType = "error";
                    return RedirectToPage("./ListLearningProfile");
                }

                // Map learner data to update request
                UpdateRequest = new UpdateLearnerRequest
                {
                    UserId = learner.UserId,
                    Email = learner.Email,
                    FirstName = learner.FirstName,
                    MiddleName = learner.MiddleName,
                    LastName = learner.LastName,
                    Dob = learner.Dob,
                    Gender = learner.Gender,
                    Phone = learner.Phone,
                    ProfileImageUrl = learner.ProfileImageUrl,
                    CccdNumber = learner.CccdNumber,
                    CccdImageFront = learner.CccdImageFront,
                    CccdImageBack = learner.CccdImageBack,
                    HealthCertificateImageUrl = learner.HealthCertificateImageUrl
                };

                // Load existing address information if available
                if (learner.AddressId.HasValue)
                {
                    var address = await _addressService.GetAddressAsync(learner.AddressId.Value);
                    if (address != null)
                    {
                        UpdateRequest.WardId = address.WardId;
                        UpdateRequest.ProvinceId = address.Ward?.ProvinceId;
                        UpdateRequest.CityId = address.Ward?.Province?.CityId;
                        UpdateRequest.HouseNumber = address.HouseNumber;
                    }
                }

                LoadAddressData();
                return Page();
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tải thông tin học viên: {ex.Message}";
                MessageType = "error";
                return RedirectToPage("./ListLearningProfile");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Custom validation for name fields - run before checking ModelState.IsValid
            if (!string.IsNullOrEmpty(UpdateRequest.FirstName) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.FirstName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("UpdateRequest.FirstName", "Họ chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!string.IsNullOrEmpty(UpdateRequest.MiddleName) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.MiddleName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("UpdateRequest.MiddleName", "Tên đệm chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!string.IsNullOrEmpty(UpdateRequest.LastName) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.LastName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("UpdateRequest.LastName", "Tên chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!ModelState.IsValid)
            {
                LoadAddressData();
                return Page();
            }

            try
            {
                var learner = await _userService.GetLearnerById(UpdateRequest.UserId);
                if (learner == null)
                {
                    Message = "Không tìm thấy học viên!";
                    MessageType = "error";
                    return RedirectToPage("./ListLearningProfile");
                }

                // Check if CCCD number already exists (exclude current user)
                if (!string.IsNullOrEmpty(UpdateRequest.CccdNumber))
                {
                    var existingUserWithCccd = await _userService.DoesUserWithCccdExistExcludingUser(UpdateRequest.CccdNumber, UpdateRequest.UserId);
                    if (existingUserWithCccd)
                    {
                        ModelState.AddModelError("UpdateRequest.CccdNumber", "Số CCCD này đã được sử dụng");
                        LoadAddressData();
                        return Page();
                    }
                }

                // Check if phone number already exists (exclude current user)
                if (!string.IsNullOrEmpty(UpdateRequest.Phone))
                {
                    var existingUserWithPhone = await _userService.DoesUserWithPhoneExistExcludingUser(UpdateRequest.Phone, UpdateRequest.UserId);
                    if (existingUserWithPhone)
                    {
                        ModelState.AddModelError("UpdateRequest.Phone", "Số điện thoại này đã được sử dụng");
                        LoadAddressData();
                        return Page();
                    }
                }

                // Check if age is valid
                if (UpdateRequest.Dob.HasValue)
                {
                    if (UpdateRequest.Dob > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                    {
                        ModelState.AddModelError("UpdateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
                        LoadAddressData();
                        return Page();
                    }

                    if (UpdateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-60)))
                    {
                        ModelState.AddModelError("UpdateRequest.Dob", "Tuổi của học viên không được quá 60.");
                        LoadAddressData();
                        return Page();
                    }
                }

                // Validate CCCD number format
                if (!string.IsNullOrEmpty(UpdateRequest.CccdNumber) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.CccdNumber, @"^\d{12}$"))
                {
                    ModelState.AddModelError("UpdateRequest.CccdNumber", "Số CCCD phải có đúng 12 chữ số và chỉ chứa số");
                    LoadAddressData();
                    return Page();
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(UpdateRequest.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(UpdateRequest.Phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    ModelState.AddModelError("UpdateRequest.Phone", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 số, bắt đầu bằng 03, 05, 07, 08, 09)");
                    LoadAddressData();
                    return Page();
                }

                // Create or update address record if ward is selected
                if (UpdateRequest.WardId.HasValue)
                {
                    if (learner.AddressId.HasValue)
                    {
                        // Update existing address
                        await _addressService.UpdateAddressAsync(
                            learner.AddressId.Value,
                            UpdateRequest.WardId.Value,
                            UpdateRequest.HouseNumber,
                            null // No road name
                        );
                    }
                    else
                    {
                        // Create new address
                        var addressId = await _addressService.CreateAddressAsync(
                            UpdateRequest.WardId.Value, 
                            UpdateRequest.HouseNumber,
                            null // No road name
                        );
                    }
                }

                // Update learner information
                await _userService.UpdateLearnerInfoAsync(UpdateRequest.UserId, UpdateRequest);

                Message = "Cập nhật thông tin học viên thành công!";
                MessageType = "success";
                return RedirectToPage("./ListLearningProfile");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi cập nhật thông tin học viên: {ex.Message}";
                MessageType = "error";
                LoadAddressData();
                return Page();
            }
        }

        public async Task<IActionResult> OnGetProvincesAsync(int cityId)
        {
            var provinces = _addressService.GetProvincesByCity(cityId);
            return new JsonResult(provinces);
        }

        public async Task<IActionResult> OnGetWardsAsync(int provinceId)
        {
            var wards = _addressService.GetWardsByProvince(provinceId);
            return new JsonResult(wards);
        }


        private void LoadAddressData()
        {
            AvailableCities = _addressService.GetAllCities();
            
            if (UpdateRequest.CityId.HasValue)
            {
                AvailableProvinces = _addressService.GetProvincesByCity(UpdateRequest.CityId.Value);
            }
            
            if (UpdateRequest.ProvinceId.HasValue)
            {
                AvailableWards = _addressService.GetWardsByProvince(UpdateRequest.ProvinceId.Value);
            }
        }
    }
} 