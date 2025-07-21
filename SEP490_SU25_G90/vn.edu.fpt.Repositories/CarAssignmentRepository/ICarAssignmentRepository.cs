using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository
{
    public interface ICarAssignmentRepository
    {
        public Task<IList<CarAssignment>> GetAllCarAssignmentsAsync();
        public Task<IList<CarAssignment>> GetAllAssignmentsByCarIdAsync(int carId);
        public Task<CarAssignment> GetAssignmentByIdAsync(int assignmentId);
        public Task AddCarAssignmentAsync(CarAssignment carAssignment);
        public Task UpdateCarAssignmentAsync(CarAssignment carAssignment);
        public Task DeleteCarAssignmentAsync(CarAssignment carAssignment);
    }
}
