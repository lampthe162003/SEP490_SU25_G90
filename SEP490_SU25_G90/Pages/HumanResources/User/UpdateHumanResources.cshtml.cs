using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.UserDto;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;

namespace SEP490_SU25_G90.Pages.HumanResources.User
{
    public class UpdateHumanResourcesModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UpdateHumanResourcesModel(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public UpdateStaffRequest UpdateRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            UpdateRequest = _mapper.Map<UpdateStaffRequest>(await _userService.GetUserDetailsAsync(id));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _userService.UpdateStaffAsync(UpdateRequest);

            return Redirect("/HumanResources/User/ListHumanResources");
        }
    }
}
