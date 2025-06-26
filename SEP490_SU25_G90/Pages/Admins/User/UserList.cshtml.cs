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

        public UserListModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
        }

        public IList<UserListInformationResponse> User { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? NameSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EmailSearch { get; set; }

        public async Task<IActionResult> OnGetAsync() { 
            User = await _userService.GetAllUsers(NameSearch, EmailSearch);

            return Page();
        }
    }
}
