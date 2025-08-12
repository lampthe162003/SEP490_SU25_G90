using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class UserDetailsModel : PageModel
    {
        private readonly IUserService _userService;
        public UserDetailsModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserListInformationResponse UserDetails { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserDetails = await _userService.GetUserDetailsAsync(id);
            return Page();
        }
    }
}
