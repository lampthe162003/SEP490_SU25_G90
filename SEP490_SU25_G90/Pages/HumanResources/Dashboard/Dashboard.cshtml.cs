using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SEP490_SU25_G90.Pages.HumanResources.Dashboard
{
    [Authorize(Roles = "human resources")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
