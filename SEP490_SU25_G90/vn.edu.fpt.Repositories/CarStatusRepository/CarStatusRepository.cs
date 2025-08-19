using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarStatusRepository
{
    public class CarStatusRepository : ICarStatusRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public CarStatusRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<IList<CarAssignmentStatus>> GetCarAssignmentStatuses()
        {
            return await _context.CarAssignmentStatuses.ToListAsync();
        }
    }
}
