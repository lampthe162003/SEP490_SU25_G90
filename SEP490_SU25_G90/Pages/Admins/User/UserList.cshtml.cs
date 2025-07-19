using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
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

        public IList<UserListInformationResponse> Users { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? NameSearch { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? EmailSearch { get; set; }

        public async Task<IActionResult> OnGetAsync() { 
            Users = await _userService.GetAllUsers(NameSearch, EmailSearch);

            return Page();
        }
    }
}
