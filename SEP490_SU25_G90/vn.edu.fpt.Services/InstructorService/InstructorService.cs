using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.EmailService;
using System.Text;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;

        public InstructorService(IInstructorRepository instructorRepository, IMapper mapper, IWebHostEnvironment env, IEmailService emailService)
        {
            _instructorRepository = instructorRepository;
            _mapper = mapper;
            _env = env;
            _emailService = emailService;
        }

        public IList<InstructorListInformationResponse> GetAllInstructors(string? name = null, byte? licenceTypeId = null)
        {
            var query = _instructorRepository.GetAllInstructors();

            if (!string.IsNullOrEmpty(name))
            {
                query = _instructorRepository.GetInstructorsByName(query, name);
            }

            if (licenceTypeId.HasValue)
            {
                query = _instructorRepository.GetInstructorsByLicenceType(query, licenceTypeId.Value);
            }

            var instructors = query.ToList();

            var result = instructors.Select(instructor => new InstructorListInformationResponse
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
                CccdNumber = instructor.Cccd?.CccdNumber,
                CccdImageUrl = instructor.Cccd?.ImageMt,
                CccdImageUrlMs = instructor.Cccd?.ImageMs,
                AddressDisplay = BuildAddressDisplay(instructor.Address),
                Specializations = instructor.InstructorSpecializations.Select(ins => new LicenceTypeResponse
                {
                    LicenceTypeId = ins.LicenceType.LicenceTypeId,
                    LicenceCode = ins.LicenceType.LicenceCode
                }).ToList(),
                //StudentCount = instructor.LearningApplicationInstructors.Count(la => la.LearningStatus == 1) // Active learning applications
            }).ToList();

            return result;
        }

        public InstructorListInformationResponse? GetInstructorById(int id)
        {
            var instructor = _instructorRepository.GetInstructorById(id);
            if (instructor == null) return null;

            return new InstructorListInformationResponse
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
                CccdNumber = instructor.Cccd?.CccdNumber,
                CccdImageUrl = instructor.Cccd?.ImageMt,
                CccdImageUrlMs = instructor.Cccd?.ImageMs,
                AddressDisplay = BuildAddressDisplay(instructor.Address),
                Specializations = instructor.InstructorSpecializations.Select(ins => new LicenceTypeResponse
                {
                    LicenceTypeId = ins.LicenceType.LicenceTypeId,
                    LicenceCode = ins.LicenceType.LicenceCode
                }).ToList(),
                //StudentCount = instructor.LearningApplicationInstructors.Count(la => la.LearningStatus == 1)
            };
        }

        public void CreateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor)
        {
            _instructorRepository.Create(instructor);
        }

        public async Task<string> CreateInstructorAsync(CreateInstructorRequest request)
        {
            // Generate random password
            string password = GenerateRandomPassword(12);

            // Handle file uploads
            if (request.ProfileImageFile != null)
            {
                request.ProfileImageUrl = await SaveImageAsync(request.ProfileImageFile);
            }

            if (request.CccdImageFrontFile != null)
            {
                request.CccdImageFront = await SaveImageAsync(request.CccdImageFrontFile);
            }

            if (request.CccdImageBackFile != null)
            {
                request.CccdImageBack = await SaveImageAsync(request.CccdImageBackFile);
            }

            // Create new instructor user
            var instructor = new SEP490_SU25_G90.vn.edu.fpt.Models.User
            {
                Email = request.Email,
                Password = password,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Dob = request.Dob,
                Gender = request.Gender,
                Phone = request.Phone,
                ProfileImageUrl = request.ProfileImageUrl
            };

            // Handle CCCD if provided
            if (!string.IsNullOrWhiteSpace(request.CccdNumber) ||
                !string.IsNullOrWhiteSpace(request.CccdImageFront) ||
                !string.IsNullOrWhiteSpace(request.CccdImageBack))
            {
                var cccd = new Cccd
                {
                    CccdNumber = request.CccdNumber ?? "",
                    ImageMt = request.CccdImageFront,
                    ImageMs = request.CccdImageBack
                };
                instructor.Cccd = cccd;
            }

            // Create instructor
            _instructorRepository.Create(instructor);

            // Add instructor role (role ID 3)
            _instructorRepository.AddUserRole(instructor.UserId, 3);

            // Add specializations
            foreach (var licenceTypeId in request.SelectedSpecializations)
            {
                AddSpecialization(instructor.UserId, licenceTypeId);
            }

            // Send email with password
            string fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}".Trim();

            try
            {
                await _emailService.SendNewAccountPasswordAsync(request.Email!, fullName, password);
            }
            catch (Exception)
            {

            }


            return password; // Return for confirmation (optional)
        }
        private string GenerateRandomPassword(int length)
        {
            const string upper = "ABCDEFGHJKMNPQRSTUVWXYZ"; // 24 ký tự
            const string lower = "abcdefghijkmnpqrstuvwxyz"; // 24 ký tự
            const string digits = "23456789"; // 8 ký tự
            const string special = "!@#$%"; // 5 ký tự

            const string all = upper + lower + digits + special;
            var random = new Random();
            var result = new StringBuilder(length);

            // Ensure at least one of each type
            result.Append(upper[random.Next(upper.Length)]);
            result.Append(lower[random.Next(lower.Length)]);
            result.Append(digits[random.Next(digits.Length)]);
            result.Append(special[random.Next(special.Length)]);

            // Fill the rest randomly
            for (int i = 4; i < length; i++)
            {
                result.Append(all[random.Next(all.Length)]);
            }

            // Shuffle the result
            for (int i = 0; i < result.Length; i++)
            {
                int j = random.Next(i, result.Length);
                (result[i], result[j]) = (result[j], result[i]);
            }

            return result.ToString();
        }

        public void UpdateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor)
        {
            _instructorRepository.Update(instructor);
        }

        public async Task UpdateInstructorInfoAsync(int instructorId, UpdateInstructorRequest request)
        {
            // Xử lý upload file ảnh
            if (request.ProfileImageFile != null)
            {
                request.ProfileImageUrl = await SaveImageAsync(request.ProfileImageFile, request.ProfileImageUrl);
            }

            if (request.CccdImageFrontFile != null)
            {
                request.CccdImageFront = await SaveImageAsync(request.CccdImageFrontFile, request.CccdImageFront);
            }

            if (request.CccdImageBackFile != null)
            {
                request.CccdImageBack = await SaveImageAsync(request.CccdImageBackFile, request.CccdImageBack);
            }

            _instructorRepository.UpdateInstructorInfo(instructorId, request);
        }

        public void DeleteInstructor(int id)
        {
            _instructorRepository.Delete(id);
        }

        public void AddSpecialization(int instructorId, byte licenceTypeId)
        {
            var specialization = new InstructorSpecialization
            {
                InstructorId = instructorId,
                LicenceTypeId = licenceTypeId
            };
            _instructorRepository.AddSpecialization(specialization);
        }

        public void RemoveSpecialization(int instructorId, byte licenceTypeId)
        {
            _instructorRepository.RemoveSpecialization(instructorId, licenceTypeId);
        }

        public List<LicenceTypeResponse> GetAllLicenceTypes()
        {
            var licenceTypes = _instructorRepository.GetAllLicenceTypes();
            return licenceTypes.Select(lt => new LicenceTypeResponse
            {
                LicenceTypeId = lt.LicenceTypeId,
                LicenceCode = lt.LicenceCode
            }).ToList();
        }

        public async Task<List<LearnerUserResponse>> GetAllLearnersAsync(string? searchString = null)
        {
            return await _instructorRepository.GetAllLearnersAsync(searchString);
        }

        private string BuildAddressDisplay(Address? address)
        {
            if (address == null) return "";

            var parts = new List<string>();

            if (!string.IsNullOrEmpty(address.HouseNumber))
                parts.Add(address.HouseNumber);

            if (!string.IsNullOrEmpty(address.RoadName))
                parts.Add(address.RoadName);

            if (address.Ward != null)
            {
                if (!string.IsNullOrEmpty(address.Ward.WardName))
                    parts.Add(address.Ward.WardName);

                if (address.Ward.Province != null)
                {
                    if (!string.IsNullOrEmpty(address.Ward.Province.ProvinceName))
                        parts.Add(address.Ward.Province.ProvinceName);

                    if (address.Ward.Province.City != null && !string.IsNullOrEmpty(address.Ward.Province.City.CityName))
                        parts.Add(address.Ward.Province.City.CityName);
                }
            }

            return string.Join(", ", parts);
        }

        public async Task<bool> UpdateLearnerScoresAsync(int learningId, int? theory, int? simulation, int? obstacle, int? practical)
        {
            return await _instructorRepository.UpdateLearnerScoresAsync(learningId, theory, simulation, obstacle, practical);
        }
        public async Task<List<LearningApplicationsResponse>> GetLearningApplicationsByInstructorAsync(int instructorId)
        {
            return await _instructorRepository.GetLearningApplicationsByInstructorAsync(instructorId);
        }
        public async Task<LearningApplicationsResponse?> GetLearningApplicationDetailAsync(int learningId)
        {
            return await _instructorRepository.GetLearningApplicationDetailAsync(learningId);
        }

        // Lưu file ảnh lên /wwwroot/uploads/instructor, xử lý trùng tên bằng cách thêm hậu tố (1), (2), ...
        private async Task<string?> SaveImageAsync(IFormFile? file, string? oldFilePath = null)
        {
            if (file == null) return oldFilePath;

            // Xoá file cũ nếu có
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                var fullOldPath = Path.Combine(_env.WebRootPath, oldFilePath.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(fullOldPath))
                {
                    System.IO.File.Delete(fullOldPath);
                }
            }

            var folder = Path.Combine(_env.WebRootPath, "uploads", "instructor");
            Directory.CreateDirectory(folder);

            // Lấy phần mở rộng gốc
            var extension = Path.GetExtension(file.FileName);

            // Sinh tên ngẫu nhiên 20 ký tự dựa trên time + Guid
            string uniqueFileName;
            string path;
            do
            {
                var randomPart = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                    .Replace("=", "").Replace("+", "").Replace("/", "");
                // Lấy 20 ký tự đầu tiên + thêm ticks cho chắc
                uniqueFileName = (DateTime.UtcNow.Ticks.ToString() + randomPart)
                                    .Substring(0, 20) + extension;

                path = Path.Combine(folder, uniqueFileName);
            }
            while (System.IO.File.Exists(path)); // vòng lặp cực hiếm khi xảy ra

            // Lưu file mới
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            // Trả về đường dẫn -> lưu DB
            return "/uploads/instructor/" + uniqueFileName;
        }



    }
}