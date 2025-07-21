using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository
{
    public class CarAssignmentRepository : ICarAssignmentRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public CarAssignmentRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task AddCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Add(carAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Remove(carAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<CarAssignment>> GetAllAssignmentsByCarIdAsync(int carId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Instructor)
                .Where(ca => ca.CarId == carId)
                .ToListAsync();
        }

        public async Task<IList<CarAssignment>> GetAllCarAssignmentsAsync()
        {
            return await _context.CarAssignments.Include(ca => ca.Instructor).ToListAsync();
        }

        public async Task<CarAssignment> GetAssignmentByIdAsync(int assignmentId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Instructor)
                .Where(ca => ca.AssignmentId == assignmentId)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Update(carAssignment);
            await _context.SaveChangesAsync();
        }
    }
}
