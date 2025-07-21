using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public interface ICarAssignmentService
    {
        public Task<IList<CarAssignmentInformationResponse>> GetAllCarAssignments();
        public Task<IList<CarAssignmentInformationResponse>> GetAssignmentsByCarId(int carId);
        public Task<CarAssignmentInformationResponse> GetAssignmentById(int assignmentId);
        public Task AddCarAssignment(CarAssignment carAssignment);
        public Task UpdateCarAssignment(CarAssignment carAssignment);
        public Task DeleteCarAssignment(CarAssignment carAssignment);
    }
}
