using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    [Authorize(Roles = "human resources")]
    public class ListHumanResourcesModel : PageModel
    {
        private readonly IUserService _userService;
        private const byte humanResourcesRoleId = 2;

        public ListHumanResourcesModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public List<UserListInformationResponse> ListHR { get; set; } = default!;

        [BindProperty]
        public string? SearchName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ListHR = await _userService.GetUsersByRole(humanResourcesRoleId);
            if (SearchName != null)
            {
                ListHR = ListHR.Where(u => u.FullName.Contains(SearchName)).ToList();
            }
            return Page();
        }
    }
}
