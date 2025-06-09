using AutoMapper;
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

        public UserService(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _mapper = mapper;
        }
        public List<UserListInformationResponse> GetAllUsers() 
            => _mapper.Map<List<UserListInformationResponse>>(_userRepository.GetAllUsers());
    }
}
