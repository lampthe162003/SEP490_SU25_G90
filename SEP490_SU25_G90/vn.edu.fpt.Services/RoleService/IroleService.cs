using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.RoleService
{
    public interface IroleService
    {
        public Task<IList<Role>> GetRolesAsync();
        public Task<Role> GetRoleById(int id);
        public Task<List<SelectListItem>> GetRolesAsSelectListAsync();
    }
}
