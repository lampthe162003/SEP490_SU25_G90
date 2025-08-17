using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;

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

        public UserService(Sep490Su25G90DbContext context,
            IMapper mapper,
            IUserRepository userRepository,
            IPasswordHasher<Models.User> hasher,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mapperConfig = mapper.ConfigurationProvider;
            _context = context;
            _hasher = hasher;
            _roleRepository = roleRepository;
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
            var query = _userRepository.GetAllUsers();

            if (!string.IsNullOrWhiteSpace(name)) query = _userRepository.GetUsersByName(query, name);
            if (!string.IsNullOrWhiteSpace(email)) query = _userRepository.GetUsersByEmail(query, email);
            var users = await query.ToListAsync();
            return _mapper.Map<List<UserListInformationResponse>>(users);
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
                    ? $" {learner.Address.Ward?.WardName}, {learner.Address.Ward?.Province?.ProvinceName}, {learner.Address.Ward?.Province?.City?.CityName}"
                    : null,
                ProfileImageUrl = learner.ProfileImageUrl,
                HealthCertificateImageUrl = learner.HealthCertificate?.ImageUrl,
                LearningApplications = learningApplicationInfos
            };

            return result;
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

        public async Task UpdateLearnerInfo(int userId, UpdateLearnerRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get learner
                var learner = await _context.Users
                    .Include(u => u.Cccd)
                    .Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync();

                if (learner == null)
                {
                    throw new InvalidOperationException("Không tìm thấy thông tin học viên");
                }

                // Update basic information
                learner.Email = request.Email;
                learner.FirstName = request.FirstName;
                learner.MiddleName = request.MiddleName;
                learner.LastName = request.LastName;
                learner.Dob = request.Dob;
                learner.Gender = request.Gender;
                learner.Phone = request.Phone;

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
            var allUsers = _userRepository.GetAllUsers();
            var users = await allUsers.Where(u => u.UserRoles.Any(ur => ur.Role.RoleId == roleId)).ToListAsync();
            return _mapper.Map<List<UserListInformationResponse>>(users);
        }

        public async Task UpdateStaffAsync(UpdateStaffRequest request)
        {
            await _userRepository.Update(_mapper.Map<User>(request));
        }
    }
}
