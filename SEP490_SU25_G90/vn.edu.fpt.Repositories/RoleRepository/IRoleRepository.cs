using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        public Task<IList<Role>> GetRolesAsync();
        public Task<List<SelectListItem>> GetRolesAsSelectListAsync();
        public Task<Role> GetRoleById(int id);
        public Task Add(Role role);
        public Task Update(Role role);
        public Task AddRoleToUser(UserRole userRole);
    }
}
