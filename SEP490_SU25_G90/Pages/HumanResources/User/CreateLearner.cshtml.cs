using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.AddressService;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using System.Text.Json.Serialization;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class CreateLearnerModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;

        public CreateLearnerModel(IUserService userService, IAddressService addressService)
        {
            _userService = userService;
            _addressService = addressService;
        }

        [BindProperty]
        public CreateLearnerRequest CreateRequest { get; set; } = new();

        public List<CityResponse> AvailableCities { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        [TempData]
        public string? MessageType { get; set; }

        // Properties for localStorage file data
        [BindProperty]
        public string? ProfileImageData { get; set; }

        [BindProperty]
        public string? CccdFrontImageData { get; set; }

        [BindProperty]
        public string? CccdBackImageData { get; set; }

        [BindProperty]
        public string? HealthCertImageData { get; set; }

        public void OnGet()
        {
            LoadAvailableCities();

            // Clear localStorage data after successful creation
            ClearLocalStorageData();

            // Set flag to clear localStorage on client-side
            TempData["ClearLocalStorage"] = "true";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Process localStorage files first
            ProcessLocalStorageFiles();

            // Custom validation for name fields - run before checking ModelState.IsValid
            if (!string.IsNullOrEmpty(CreateRequest.FirstName) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.FirstName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("CreateRequest.FirstName", "Họ chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!string.IsNullOrEmpty(CreateRequest.MiddleName) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.MiddleName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("CreateRequest.MiddleName", "Tên đệm chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!string.IsNullOrEmpty(CreateRequest.LastName) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.LastName, @"^[\p{L}]+$"))
            {
                ModelState.AddModelError("CreateRequest.LastName", "Tên chỉ được chứa chữ cái và không được có khoảng trắng hoặc số");
            }

            if (!ModelState.IsValid)
            {
                LoadAddressData();
                return Page();
            }

            try
            {

                // Check if email already exists
                var existingUserWithEmail = await _userService.DoesUserWithEmailExist(CreateRequest.Email!);
                if (existingUserWithEmail)
                {
                    ModelState.AddModelError("CreateRequest.Email", "Email này đã được sử dụng");
                    LoadAddressData();
                    return Page();
                }

                // Check if CCCD number already exists
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber))
                {
                    var existingUserWithCccd = await _userService.DoesUserWithCccdExist(CreateRequest.CccdNumber);
                    if (existingUserWithCccd)
                    {
                        ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD này đã được sử dụng");
                        LoadAddressData();
                        return Page();
                    }
                }

                // Check if phone number already exists
                if (!string.IsNullOrEmpty(CreateRequest.Phone))
                {
                    var existingUserWithPhone = await _userService.DoesUserWithPhoneExist(CreateRequest.Phone);
                    if (existingUserWithPhone)
                    {
                        ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại này đã được sử dụng");
                        LoadAddressData();
                        return Page();
                    }
                }

                // Check if age is lower than 18
                if (CreateRequest.Dob > DateOnly.FromDateTime(DateTime.Today.AddYears(-18)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên chưa đủ 18.");
                    LoadAddressData();
                    return Page();
                }

                // Check if age is too high (over 60 for learners)
                if (CreateRequest.Dob < DateOnly.FromDateTime(DateTime.Today.AddYears(-60)))
                {
                    ModelState.AddModelError("CreateRequest.Dob", "Tuổi của học viên không được quá 60.");
                    LoadAddressData();
                    return Page();
                }

                // Validate CCCD number format
                if (!string.IsNullOrEmpty(CreateRequest.CccdNumber) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.CccdNumber, @"^\d{12}$"))
                {
                    ModelState.AddModelError("CreateRequest.CccdNumber", "Số CCCD phải có đúng 12 chữ số và chỉ chứa số");
                    LoadAddressData();
                    return Page();
                }

                // Validate phone number format
                if (!string.IsNullOrEmpty(CreateRequest.Phone) && !System.Text.RegularExpressions.Regex.IsMatch(CreateRequest.Phone, @"^(0[3|5|7|8|9])[0-9]{8}$"))
                {
                    ModelState.AddModelError("CreateRequest.Phone", "Số điện thoại không hợp lệ. Vui lòng nhập số điện thoại Việt Nam hợp lệ (10 số, bắt đầu bằng 03, 05, 07, 08, 09)");
                    LoadAddressData();
                    return Page();
                }



                var password = await _userService.CreateLearnerAsync(CreateRequest);
                Message = $"Tạo tài khoản học viên thành công! Mật khẩu đã được gửi về email {CreateRequest.Email}";
                MessageType = "success";

                // Clear localStorage data after successful creation
                ClearLocalStorageData();

                // Set flag to clear localStorage on client-side
                TempData["ClearLocalStorage"] = "true";

                return RedirectToPage("./ListLearningProfile");
            }
            catch (Exception ex)
            {
                Message = $"Lỗi khi tạo tài khoản học viên: {ex.Message}";
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


        private void LoadAvailableCities()
        {
            AvailableCities = _addressService.GetAllCities();
        }

        private void LoadAddressData()
        {
            AvailableCities = _addressService.GetAllCities();

            if (CreateRequest.CityId.HasValue)
            {
                AvailableProvinces = _addressService.GetProvincesByCity(CreateRequest.CityId.Value);
            }

            if (CreateRequest.ProvinceId.HasValue)
            {
                AvailableWards = _addressService.GetWardsByProvince(CreateRequest.ProvinceId.Value);
            }
        }

        public List<ProvinceResponse> AvailableProvinces { get; set; } = new();
        public List<WardResponse> AvailableWards { get; set; } = new();

        private void ProcessLocalStorageFiles()
        {
            Console.WriteLine("=== ProcessLocalStorageFiles START ===");

            // Process health certificate image from localStorage
            if (!string.IsNullOrEmpty(HealthCertImageData))
            {
                Console.WriteLine($"Processing health certificate image from localStorage, data length: {HealthCertImageData.Length}");
                CreateRequest.HealthCertificateImageFile = ConvertBase64ToFormFile(HealthCertImageData, "health-cert");
                Console.WriteLine($"Health certificate image processed: {CreateRequest.HealthCertificateImageFile?.FileName}, Size: {CreateRequest.HealthCertificateImageFile?.Length}");
            }

            // Process profile image from localStorage
            if (!string.IsNullOrEmpty(ProfileImageData))
            {
                Console.WriteLine($"Processing profile image from localStorage, data length: {ProfileImageData.Length}");
                CreateRequest.ProfileImageFile = ConvertBase64ToFormFile(ProfileImageData, "profile");
                Console.WriteLine($"Profile image processed: {CreateRequest.ProfileImageFile?.FileName}, Size: {CreateRequest.ProfileImageFile?.Length}");
            }

            // Process CCCD front image from localStorage
            if (!string.IsNullOrEmpty(CccdFrontImageData))
            {
                Console.WriteLine($"Processing CCCD front image from localStorage, data length: {CccdFrontImageData.Length}");
                CreateRequest.CccdImageFrontFile = ConvertBase64ToFormFile(CccdFrontImageData, "cccd-front");
                Console.WriteLine($"CCCD front image processed: {CreateRequest.CccdImageFrontFile?.FileName}, Size: {CreateRequest.CccdImageFrontFile?.Length}");
            }

            // Process CCCD back image from localStorage
            if (!string.IsNullOrEmpty(CccdBackImageData))
            {
                Console.WriteLine($"Processing CCCD back image from localStorage, data length: {CccdBackImageData.Length}");
                CreateRequest.CccdImageBackFile = ConvertBase64ToFormFile(CccdBackImageData, "cccd-back");
                Console.WriteLine($"CCCD back image processed: {CreateRequest.CccdImageBackFile?.FileName}, Size: {CreateRequest.CccdImageBackFile?.Length}");
            }

            Console.WriteLine("=== ProcessLocalStorageFiles END ===");
        }

        private IFormFile? ConvertBase64ToFormFile(string base64Data, string fileType)
        {
            try
            {
                // Parse JSON from localStorage
                var fileData = System.Text.Json.JsonSerializer.Deserialize<LocalStorageFileData>(base64Data);
                if (fileData == null || string.IsNullOrEmpty(fileData.Data))
                    return null;

                // Extract base64 content (remove data:image/...;base64, prefix)
                var base64Content = fileData.Data;
                if (base64Content.Contains(","))
                {
                    base64Content = base64Content.Split(',')[1];
                }

                // Convert base64 to byte array
                var fileBytes = Convert.FromBase64String(base64Content);

                // Create memory stream
                var stream = new MemoryStream(fileBytes);

                // Create IFormFile
                return new FormFile(stream, 0, fileBytes.Length, fileType, fileData.Name)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = fileData.Type
                };
            }
            catch (Exception ex)
            {
                // Log error but don't break the flow
                Console.WriteLine($"Error converting localStorage file data: {ex.Message}");
                return null;
            }
        }

        private void ClearLocalStorageData()
        {
            // Clear the properties that contain localStorage data
            HealthCertImageData = null;
            ProfileImageData = null;
            CccdFrontImageData = null;
            CccdBackImageData = null;
        }

        // Class to deserialize localStorage file data
        private class LocalStorageFileData
        {
            [JsonPropertyName("data")]
            public string Data { get; set; } = "";

            [JsonPropertyName("name")]
            public string Name { get; set; } = "";

            [JsonPropertyName("type")]
            public string Type { get; set; } = "";

            [JsonPropertyName("size")]
            public long Size { get; set; }

            [JsonPropertyName("lastModified")]
            public long LastModified { get; set; }
        }
    }
}