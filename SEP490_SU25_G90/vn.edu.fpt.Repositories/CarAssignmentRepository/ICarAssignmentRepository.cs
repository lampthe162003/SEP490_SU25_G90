
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository
{
    public interface ICarAssignmentRepository
    {
        Task<List<CarAssignment>> GetAllCarAssignmentsAsync();
        Task<List<CarAssignment>> GetAllCarsWithAssignmentsAsync();
        Task<CarAssignment?> GetCarAssignmentByIdAsync(int assignmentId);
        Task<List<CarAssignment>> GetCarAssignmentsByInstructorAsync(int instructorId);
        Task<List<CarAssignment>> GetCarAssignmentsByDateAsync(DateOnly date);
        Task<List<CarAssignment>> GetCarAssignmentsByLicenceTypeAsync(byte licenceTypeId);
        Task<List<CarAssignment>> SearchCarAssignmentsAsync(string? carMake, DateOnly? scheduleDate);
        Task<CarAssignment> CreateCarAssignmentAsync(CarAssignment carAssignment);
        Task<bool> UpdateCarAssignmentAsync(CarAssignment carAssignment);
        Task<bool> DeleteCarAssignmentAsync(int assignmentId);
        Task<List<Car>> GetAvailableCarsAsync(DateOnly date, int slotId);
        Task<List<ScheduleSlot>> GetAllScheduleSlotsAsync();
        Task<List<LicenceType>> GetAllLicenceTypesAsync();
        Task<List<Car>> GetAllCarsAsync();
        Task<bool> IsCarAvailableAsync(int carId, DateOnly date, int slotId);

        public Task<IList<CarAssignment>> GetAllCarAssignmentsAsync();
        public Task<IList<CarAssignment>> GetAllAssignmentsByCarIdAsync(int carId);
        public Task<CarAssignment> GetAssignmentByIdAsync(int assignmentId);
        public Task AddCarAssignmentAsync(CarAssignment carAssignment);
        public Task UpdateCarAssignmentAsync(CarAssignment carAssignment);
        public Task DeleteCarAssignmentAsync(CarAssignment carAssignment);
    }
}

