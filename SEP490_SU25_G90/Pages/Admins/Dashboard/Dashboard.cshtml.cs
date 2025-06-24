using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SEP490_SU25_G90.Pages.Admins.Dashboard
{
    [Authorize(Roles = "admin")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
