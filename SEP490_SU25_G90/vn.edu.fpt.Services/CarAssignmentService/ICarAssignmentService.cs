using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public interface ICarAssignmentService
    {
        public Task<IList<CarAssignment>> GetAllCarAssignments();
        public Task<IList<CarAssignment>> GetAssignmentsByCarId(int carId);
        public Task<CarAssignment> GetAssignmentById(int assignmentId);
        public Task AddCarAssignment(CarAssignment carAssignment);
        public Task UpdateCarAssignment(CarAssignment carAssignment);
        public Task DeleteCarAssignment(CarAssignment carAssignment);
    }
}
