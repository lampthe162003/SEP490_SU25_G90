using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    public class HumanResourcesDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public HumanResourcesDetailModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserDetailsInformationResponse HrStaff { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            HrStaff = await _userService.GetUserDetailsAsync(id);

            return Page();
        }
    }
}
