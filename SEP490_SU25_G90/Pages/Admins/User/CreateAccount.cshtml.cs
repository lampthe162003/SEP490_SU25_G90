using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.RoleService;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.Admins.User
{
    [Authorize(Roles = "admin")]
    public class CreateAccountModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IroleService _roleService;

        public CreateAccountModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context, IUserService service, IMapper mapper, IroleService roleService)
        {
            _context = context;
            _userService = service;
            _mapper = mapper;
            _roleService = roleService;
        }

        public List<SelectListItem> GenderSelect = new List<SelectListItem>
        {
            new SelectListItem { Value = "true", Text = "Nam" },
            new SelectListItem { Value = "false", Text = "Nữ" }
        };

        public List<SelectListItem> RoleSelect;

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Hãy chọn vai trò hợp lệ")]
        public byte Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RoleSelect = await _roleService.GetRolesAsSelectListAsync();
            return Page();
        }

        [BindProperty]
        public AccountCreationRequest User { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userService.CreateAccount(User, Role);

            return RedirectToPage("./UserList");
        }
    }
}
