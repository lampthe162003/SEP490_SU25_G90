using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public interface ICarAssignmentService
    {
        Task<List<CarAssignmentResponse>> GetAllCarAssignmentsAsync();
        Task<List<CarAssignmentResponse>> GetAllCarsWithAssignmentsAsync();
        Task<List<CarAssignmentResponse>> SearchCarAssignmentsAsync(CarAssignmentSearchRequest request, int? currentInstructorId = null);
        Task<CarAssignmentResponse?> GetCarAssignmentByIdAsync(int assignmentId);
        Task<bool> RentCarAsync(CarRentalRequest request);
        Task<bool> ReturnCarAsync(int assignmentId);
        Task<bool> UpdateCarAssignmentAsync(int assignmentId, CarRentalRequest request);
        Task<bool> DeleteCarAssignmentAsync(int assignmentId);
        
        // Helper methods cho dropdown lists
        Task<List<SelectListItem>> GetCarMakesSelectListAsync();
        Task<List<SelectListItem>> GetScheduleSlotsSelectListAsync();
        Task<List<SelectListItem>> GetAvailableCarsSelectListAsync(DateOnly date, int slotId);
        Task<List<SelectListItem>> GetInstructorsSelectListAsync();
        
        // Validation methods
        Task<bool> IsCarAvailableAsync(int carId, DateOnly date, int slotId);
        Task<bool> CanInstructorRentCarAsync(int instructorId, DateOnly date, int slotId);
        Task<List<CarAssignmentResponse>> GetCarAssignmentsByInstructorAsync(int instructorId, DateOnly date, int slotId);

        public Task<IList<CarAssignmentInformationResponse>> GetAllCarAssignments();
        public Task<IList<CarAssignmentInformationResponse>> GetAssignmentsByCarId(int carId);
        public Task<CarAssignmentInformationResponse> GetAssignmentById(int assignmentId);
        public Task AddCarAssignment(CarAssignment carAssignment);
        public Task UpdateCarAssignment(CarAssignment carAssignment);
        public Task DeleteCarAssignment(CarAssignment carAssignment);
    }
}

