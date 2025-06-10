using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.Admins.User
{
    public class UserListModel : PageModel
    {
        private readonly IUserService _userService;

        public UserListModel(Sep490Su25G90DbContext context, IMapper mapper)
        {
            _userService = new UserService(context, mapper);
        }

        public IList<UserListInformationResponse> User { get;set; } = default!;

        public void OnGet()
        {
            User = _userService.GetAllUsers();
        }
    }
}
