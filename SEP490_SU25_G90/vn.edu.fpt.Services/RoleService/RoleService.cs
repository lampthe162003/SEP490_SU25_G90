using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.RoleService
{
    public class RoleService : IroleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleRepository.GetRoleById(id);
        }

        public async Task<List<SelectListItem>> GetRolesAsSelectListAsync()
        {
            return await _roleRepository.GetRolesAsSelectListAsync();
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            return await _roleRepository.GetRolesAsync();
        }
    }
}
