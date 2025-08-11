using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Commons;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Services.User;

namespace SEP490_SU25_G90.Pages.Commons
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly JwtTokenGenerator _jwt;

        public LoginModel(IUserService service, JwtTokenGenerator jwt)
        {
            _jwt = jwt;
            _userService = service;
        }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Email không hợp lệ. Vui lòng thử lại.")]
        [Required(ErrorMessage = "Email không được để trống")]
        public String Email { get; set; } = default!;
        
        [BindProperty]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự.")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public String Password { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public bool SavePasswordCheck { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetLoginDetails(Email, Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng. Vui lòng thử lại.");
                return Page();
            }
            string? role = user.UserRoles.First().Role.RoleName;
            if (role == null)
            {
                return Redirect("/Error403");
            }

            try
            {
                var token = _jwt.GenerateToken(user.UserId, Email, role, SavePasswordCheck);
                Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true });
                if (role.Equals("academic affairs", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect("/AcademicAffairs/LearningApplications/List");
                }
                else if (role.Equals("human resources", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect("/HR/Dashboard");
                }
                else if (role.Equals("instructor", StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect("Instructor/LearningMaterial/List");
                }

                else return Redirect("./Learner/News/ListNews");
            }
            catch (InvalidOperationException)
            {
                return Redirect("/Error500");
            }
        }
    }
}
