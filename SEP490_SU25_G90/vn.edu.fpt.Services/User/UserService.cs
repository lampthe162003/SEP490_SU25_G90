using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Metadata;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _mapperConfig;

        public UserService(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _mapper = mapper;
            _mapperConfig = mapper.ConfigurationProvider;
        }
        public IList<UserListInformationResponse> GetAllUsers(string? name, string? email)
        {
            var query = _userRepository.GetAllUsers();

            if (!string.IsNullOrWhiteSpace(name)) query = _userRepository.GetUsersByName(query, name);
            if (!string.IsNullOrWhiteSpace(email)) query = _userRepository.GetUsersByEmail(query, email);
            return _mapper.Map<List<UserListInformationResponse>>(query.ToList());
        }

        public LoginInformationResponse GetLoginDetails(string email, string password)
            => _mapper.Map<LoginInformationResponse>(_userRepository.GetLoginDetails(email, password));

        public void CreateAccount(AccountCreationRequest request)
        {
            
        }
    }
}
