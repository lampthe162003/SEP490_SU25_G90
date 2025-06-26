using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.RoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public RoleRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task Add(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<IList<Role>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task Update(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetRolesAsSelectListAsync()
        {
            return await _context.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.RoleId.ToString(),
                    Text = r.RoleName
                })
                .ToListAsync();
        }

        public async Task AddRoleToUser(UserRole userRole)
        {
            await _context.UserRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
