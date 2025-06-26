using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.User
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

                user.PasswordHash = _hasher.HashPassword(user, request.PasswordHash);

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

    }
}
