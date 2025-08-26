using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.AddressService;
using SEP490_SU25_G90.vn.edu.fpt.Services.EmailService;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _mapperConfig;
        private readonly Sep490Su25G90DbContext _context;
        private readonly IPasswordHasher<Models.User> _hasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;
        private readonly IAddressService _addressService;
        private object context;
        private object hasher;
        private object roleRepository;
        private object env;
        private object emailService;

        public UserService(Sep490Su25G90DbContext context,
            IMapper mapper,
            IUserRepository userRepository,
            IPasswordHasher<Models.User> hasher,
            IRoleRepository roleRepository,
            IWebHostEnvironment env,
            IEmailService emailService, IAddressService addressService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mapperConfig = mapper.ConfigurationProvider;
            _context = context;
            _hasher = hasher;
            _roleRepository = roleRepository;
            _env = env;
            _emailService = emailService;
            _addressService = addressService;
        }

        public UserService(object context, IMapper mapper, IUserRepository userRepository, object hasher, object roleRepository, object env, object emailService)
        {
            this.context = context;
            _mapper = mapper;
            _userRepository = userRepository;
            this.hasher = hasher;
            this.roleRepository = roleRepository;
            this.env = env;
            this.emailService = emailService;
        }

        public async Task CreateAccount(AccountCreationRequest request, byte roleId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = _mapper.Map<vn.edu.fpt.Models.User>(request);

                var createdUser = await _userRepository.Create(user);

                var userRole = new UserRole
                {
                    UserId = createdUser.UserId,
                    RoleId = roleId
                };
                await _roleRepository.AddRoleToUser(userRole);

                await _userRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IList<UserListInformationResponse>> GetAllUsers(string? name, string? email)
        {
            var users = _mapper.Map<List<UserListInformationResponse>>(await _userRepository.GetAllUsers());

            if (name != null) users = users.Where(u => u.FullName.Contains(name.Trim())).ToList();
            if (email != null) users = users.Where(u => u.Email.Contains(email.Trim())).ToList();

            return users;
        }

        public async Task<LoginInformationResponse> GetLoginDetails(string email, string password)
            => _mapper.Map<LoginInformationResponse>(await _userRepository.GetLoginDetails(email, password));

        public async Task<List<LearnerUserResponse>> GetAllLearnersAsync()
        {
            var learners = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Cccd)
                .Where(u => u.UserRoles.Any(ur => ur.Role.RoleName == "learner"))
                .Select(u => new LearnerUserResponse
                {
                    UserId = u.UserId,
                    FullName = $"{u.FirstName} {u.MiddleName} {u.LastName}".Trim(),
                    Email = u.Email ?? string.Empty,
                    Phone = u.Phone ?? string.Empty,
                    CccdNumber = u.Cccd != null ? u.Cccd.CccdNumber : string.Empty,
                    Dob = u.Dob,
                    Gender = u.Gender,
                    CccdImageUrl = u.Cccd != null ? u.Cccd.ImageMt ?? string.Empty : string.Empty,
                    ProfileImageUrl = u.ProfileImageUrl ?? string.Empty
                })
                .ToListAsync();

            return learners;
        }

        public async Task<LearnerDetailResponse?> GetLearnerById(int id)
        {
            var learner = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Cccd)
                .Include(u => u.HealthCertificate)
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Where(u => u.UserId == id && u.UserRoles.Any(ur => ur.Role.RoleName == "learner"))
                .FirstOrDefaultAsync();

            if (learner == null) return null;

            // Get learning applications for this learner
            var learningApplications = await _context.LearningApplications
                .Include(la => la.LicenceType)
                .Where(la => la.LearnerId == id)
                .ToListAsync();

            var learningApplicationInfos = new List<LearningApplicationInfo>();

            foreach (var la in learningApplications)
            {
                // Find class member for this learner and licence type
                var classMember = await _context.ClassMembers
                    .Include(cm => cm.Class)
                        .ThenInclude(c => c.Instructor)
                    .Include(cm => cm.Class)
                    .Where(cm => cm.LearnerId == id &&
                                cm.Class != null)
                    .FirstOrDefaultAsync();

                var applicationInfo = new LearningApplicationInfo
                {
                    ApplicationId = la.LearningId,
                    LicenceCode = la.LicenceType?.LicenceCode ?? "",
                    LicenceName = GetLicenceName(la.LicenceType?.LicenceCode),
                    Status = GetLearningStatusName(la.LearningStatus),
                    StatusBadgeClass = GetStatusBadgeClass(la.LearningStatus),
                    AppliedDate = la.SubmittedAt,
                    CompletedDate = null, // LearningApplication doesn't have CompletedAt field
                    InstructorName = classMember?.Class?.Instructor != null
                        ? $"{classMember.Class.Instructor.FirstName} {classMember.Class.Instructor.MiddleName} {classMember.Class.Instructor.LastName}".Trim()
                        : null,
                    ClassName = classMember?.Class?.ClassName,
                    TheoryScore = la.TheoryScore,
                    SimulationScore = la.SimulationScore,
                    ObstacleScore = la.ObstacleScore,
                    PracticalScore = la.PracticalScore
                };

                learningApplicationInfos.Add(applicationInfo);
            }

            var result = new LearnerDetailResponse
            {
                UserId = learner.UserId,
                FullName = $"{learner.FirstName} {learner.MiddleName} {learner.LastName}".Trim(),
                FirstName = learner.FirstName,
                MiddleName = learner.MiddleName,
                LastName = learner.LastName,
                Email = learner.Email,
                Phone = learner.Phone,
                Dob = learner.Dob,
                Gender = learner.Gender,
                CccdNumber = learner.Cccd?.CccdNumber,
                CccdImageFront = learner.Cccd?.ImageMt,
                CccdImageBack = learner.Cccd?.ImageMs,
                AddressDisplay = learner.Address != null
                    ? BuildAddressDisplay(learner.Address)
                    : null,
                ProfileImageUrl = learner.ProfileImageUrl,
                HealthCertificateImageUrl = learner.HealthCertificate?.ImageUrl,
                LearningApplications = learningApplicationInfos,
                AddressId = learner.AddressId
            };

            return result;
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

        public async Task<string> CreateLearnerAsync(CreateLearnerRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Generate random password
                string password = GenerateRandomPassword(12);
                string hashedPassword = _hasher.HashPassword(new Models.User(), password);

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

                if (request.HealthCertificateImageFile != null)
                {
                    request.HealthCertificateImageUrl = await SaveImageAsync(request.HealthCertificateImageFile);
                }

                // Create User
                var newUser = new Models.User
                {
                    Email = request.Email,
                    Password = hashedPassword,
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Dob = request.Dob,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    ProfileImageUrl = request.ProfileImageUrl
                };
                // Create address record first if ward is selected
                if (request.WardId.HasValue)
                {
                    var addressId = await _addressService.CreateAddressAsync(
                        request.WardId.Value,
                        request.HouseNumber,
                        null // No road name
                    );
                    newUser.AddressId = addressId;
                }
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Find learner role
                var learnerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == "learner");
                if (learnerRole == null)
                {
                    throw new InvalidOperationException("Không tìm thấy role learner trong hệ thống");
                }

                // Add learner role
                var userRole = new UserRole
                {
                    UserId = newUser.UserId,
                    RoleId = learnerRole.RoleId
                };
                _context.UserRoles.Add(userRole);

                // Create CCCD if provided
                if (!string.IsNullOrEmpty(request.CccdNumber) ||
                    !string.IsNullOrEmpty(request.CccdImageFront) ||
                    !string.IsNullOrEmpty(request.CccdImageBack))
                {
                    var newCccd = new Cccd
                    {
                        CccdNumber = request.CccdNumber,
                        ImageMt = request.CccdImageFront,
                        ImageMs = request.CccdImageBack
                    };

                    _context.Cccds.Add(newCccd);
                    await _context.SaveChangesAsync();

                    newUser.CccdId = newCccd.CccdId;
                }

                // Create Health Certificate if provided
                if (!string.IsNullOrEmpty(request.HealthCertificateImageUrl))
                {
                    var newHealthCert = new HealthCertificate
                    {
                        ImageUrl = request.HealthCertificateImageUrl
                    };

                    _context.HealthCertificates.Add(newHealthCert);
                    await _context.SaveChangesAsync();

                    newUser.HealthCertificateId = newHealthCert.HealthCertificateId;
                }

                await _context.SaveChangesAsync();

                // Send email with password
                var fullName = $"{request.FirstName} {request.MiddleName} {request.LastName}".Trim();

                try
                {
                    await _emailService.SendNewAccountPasswordAsync(request.Email!, fullName, password);
                }
                catch (Exception)
                {

                }

                await transaction.CommitAsync();
                return password;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Generate random password
        private string GenerateRandomPassword(int length)
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*";

            var random = new Random();
            var password = new List<char>();

            // Đảm bảo có ít nhất 1 ký tự từ mỗi loại
            password.Add(lowercase[random.Next(lowercase.Length)]);
            password.Add(uppercase[random.Next(uppercase.Length)]);
            password.Add(digits[random.Next(digits.Length)]);
            password.Add(specialChars[random.Next(specialChars.Length)]);

            // Thêm các ký tự còn lại ngẫu nhiên
            string allChars = lowercase + uppercase + digits + specialChars;
            for (int i = 4; i < length; i++)
            {
                password.Add(allChars[random.Next(allChars.Length)]);
            }

            // Trộn ngẫu nhiên các ký tự
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        private string GetLearningStatusName(byte? status)
        {
            return status switch
            {
                1 => "Chưa bắt đầu",
                2 => "Đang học",
                3 => "Hoàn thành",
                4 => "Đã hủy",
                _ => "Không xác định"
            };
        }

        private string GetStatusBadgeClass(byte? status)
        {
            return status switch
            {
                1 => "bg-secondary",
                2 => "bg-info",
                3 => "bg-success",
                4 => "bg-danger",
                _ => "bg-secondary"
            };
        }

        private string GetLicenceName(string? licenceCode)
        {
            return licenceCode switch
            {
                "A1" => "Xe mô tô hai bánh có dung tích xi-lanh từ 50 cm³ đến dưới 175 cm³",
                "A2" => "Xe mô tô hai bánh có dung tích xi-lanh từ 175 cm³ trở lên và xe mô tô ba bánh",
                "A3" => "Xe mô tô hai bánh có dung tích xi-lanh từ 175 cm³ trở lên",
                "A4" => "Xe mô tô ba bánh có khối lượng không vượt quá 400kg",
                "B1" => "Xe ô tô không hành nghề lái xe",
                "B2" => "Xe ô tô hành nghề lái xe",
                "C" => "Xe ô tô tải có trọng tải thiết kế từ 3.500 kg trở lên",
                "D" => "Xe ô tô chở người từ 10 đến 30 chỗ ngồi",
                "E" => "Xe ô tô chở người từ 31 chỗ ngồi trở lên",
                "F" => "Các loại xe quy định cho giấy phép lái xe hạng D và được kéo theo một rơ-moóc",
                _ => licenceCode ?? "Không xác định"
            };
        }

        public async Task UpdateLearnerInfoAsync(int userId, UpdateLearnerRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                if (request.HealthCertificateImageFile != null)
                {
                    request.HealthCertificateImageUrl = await SaveImageAsync(request.HealthCertificateImageFile, request.HealthCertificateImageUrl);
                }

                // Get learner
                var learner = await _context.Users
                    .Include(u => u.Cccd)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (learner == null)
                {
                    return;
                }

                // Update basic information
                learner.Email = request.Email;
                learner.FirstName = request.FirstName;
                learner.MiddleName = request.MiddleName;
                learner.LastName = request.LastName;
                learner.Dob = request.Dob;
                learner.Gender = request.Gender;
                learner.Phone = request.Phone;
                learner.AddressId = request.AddressId;
                // Update ProfileImageUrl if provided
                if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl))
                {
                    learner.ProfileImageUrl = request.ProfileImageUrl;
                }

                // Update or create CCCD
                if (!string.IsNullOrEmpty(request.CccdNumber) ||
                    !string.IsNullOrEmpty(request.CccdImageFront) ||
                    !string.IsNullOrEmpty(request.CccdImageBack))
                {
                    if (learner.Cccd != null)
                    {
                        // Update existing CCCD
                        learner.Cccd.CccdNumber = request.CccdNumber ?? learner.Cccd.CccdNumber;
                        learner.Cccd.ImageMt = request.CccdImageFront ?? learner.Cccd.ImageMt;
                        learner.Cccd.ImageMs = request.CccdImageBack ?? learner.Cccd.ImageMs;
                    }
                    else
                    {
                        // Create new CCCD
                        var newCccd = new Cccd
                        {
                            CccdNumber = request.CccdNumber,
                            ImageMt = request.CccdImageFront,
                            ImageMs = request.CccdImageBack
                        };

                        _context.Cccds.Add(newCccd);
                        await _context.SaveChangesAsync(); // Save to get CCCD ID

                        learner.CccdId = newCccd.CccdId;
                    }
                }

                // Update or create Health Certificate
                if (!string.IsNullOrEmpty(request.HealthCertificateImageUrl))
                {
                    if (learner.HealthCertificate != null)
                    {
                        // Update existing Health Certificate
                        learner.HealthCertificate.ImageUrl = request.HealthCertificateImageUrl;
                    }
                    else
                    {
                        // Create new Health Certificate
                        var newHealthCert = new HealthCertificate
                        {
                            ImageUrl = request.HealthCertificateImageUrl
                        };

                        _context.HealthCertificates.Add(newHealthCert);
                        await _context.SaveChangesAsync(); // Save to get Health Certificate ID

                        learner.HealthCertificateId = newHealthCert.HealthCertificateId;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Lưu file ảnh lên /wwwroot/uploads/learner, xử lý trùng tên bằng cách thêm hậu tố (1), (2), ...
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

            var folder = Path.Combine(_env.WebRootPath, "uploads", "learner");
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
            return "/uploads/learner/" + uniqueFileName;
        }


        public async Task UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await (_userRepository.GetUserById(userId) ?? throw new ArgumentNullException("Người dùng không tồn tại."));
            user.Password = newPassword;
            await _userRepository.Update(user);
        }

        public async Task<UserDetailsInformationResponse> GetUserDetailsAsync(int userId)
        {
            var user = _mapper.Map<UserDetailsInformationResponse>(await _userRepository.GetUserById(userId));
            return user;
        }

        public async Task<LoginInformationResponse> GetLoginDetailsByEmail(string email)
        {
            var user = _mapper.Map<LoginInformationResponse>(await _userRepository.GetUserByEmail(email));
            return user;
        }

        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _userRepository.GetUserByEmail(email);
            user.Password = newPassword;
            await _userRepository.Update(user);
        }

        public async Task<bool> DoesUserWithEmailExist(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user != null;
        }

        public async Task<List<UserListInformationResponse>> GetUsersByRole(byte roleId)
        {
            var allUsers = await _userRepository.GetAllUsers();
            var users = allUsers.Where(u => u.UserRoles.Any(ur => ur.Role.RoleId == roleId)).ToList();
            return _mapper.Map<List<UserListInformationResponse>>(users);
        }

        public async Task UpdateStaffAsync(UpdateStaffRequest request)
        {
            await _userRepository.Update(_mapper.Map<User>(request));
        }

        public async Task<bool> DoesUserWithCccdExist(string cccdNumber)
        {
            var users = await _userRepository.GetAllUsers();
            return users.Any(u => u.Cccd != null && u.Cccd.CccdNumber == cccdNumber);
        }

        public async Task<bool> DoesUserWithPhoneExist(string phoneNumber)
        {
            var users = await _userRepository.GetAllUsers();
            return users.Any(u => u.Phone == phoneNumber);
        }

        public async Task<bool> DoesUserWithCccdExistExcludingUser(string cccdNumber, int excludeUserId)
        {
            var users = await _userRepository.GetAllUsers();
            return users.Any(u => u.UserId != excludeUserId && u.Cccd != null && u.Cccd.CccdNumber == cccdNumber);
        }

        public async Task<bool> DoesUserWithPhoneExistExcludingUser(string phoneNumber, int excludeUserId)
        {
            var users = await _userRepository.GetAllUsers();
            return users.Any(u => u.UserId != excludeUserId && u.Phone == phoneNumber);
        }
    }
}
