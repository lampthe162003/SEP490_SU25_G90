using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.Admins.User
{
    public class CreateAccountModel : PageModel
    {
        private readonly SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CreateAccountModel(SEP490_SU25_G90.vn.edu.fpt.Models.Sep490Su25G90DbContext context, IUserService service, IMapper mapper)
        {
            _context = context;
            _userService = service;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
        //ViewData["AddressId"] = new SelectList(_context.Addresses, "AddressId", "AddressId");
        //ViewData["CccdId"] = new SelectList(_context.Cccds, "CccdId", "CccdId");
        //ViewData["HealthCertificateId"] = new SelectList(_context.HealthCertificates, "HealthCertificateId", "HealthCertificateId");
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

            _context.Users.Add(_mapper.Map<vn.edu.fpt.Models.User>(User));
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Users/List");
        }
    }
}
