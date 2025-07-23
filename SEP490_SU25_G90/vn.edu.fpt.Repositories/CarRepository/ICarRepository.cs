using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository
{
    public interface ICarRepository
    {
        public Task AddCarAsync(Car car);
        
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int carId);
        Task<List<Car>> SearchCarsAsync(string? carMake, string? carModel, string? licensePlate);
        Task<Car> CreateCarAsync(Car car);
        Task<bool> UpdateCarAsync(Car car);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> IsLicensePlateExistsAsync(string licensePlate, int? excludeCarId = null);
        Task<int> GetCarAssignmentCountAsync(int carId);
        Task<bool> DeleteCarAssignmentsAsync(int carId);
        Task<List<CarAssignment>> GetActiveCarAssignmentsAsync(int carId);
    }
} 
