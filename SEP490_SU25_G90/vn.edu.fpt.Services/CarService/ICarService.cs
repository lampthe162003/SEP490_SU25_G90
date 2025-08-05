using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarService
{
    public interface ICarService
    {
        public Task<IList<Car>> GetAllCars();
        public Task<Car> GetCarById(int carId);
        public Task AddCar(Car car);
        public Task UpdateCar(Car car);
        
        Task<List<CarResponse>> GetAllCarsAsync();
        Task<List<CarResponse>> SearchCarsAsync(CarSearchRequest request);
        Task<CarResponse?> GetCarByIdAsync(int carId);
        Task<bool> CreateCarAsync(CarRequest request);
        Task<bool> UpdateCarAsync(int carId, CarRequest request);
        Task<bool> DeleteCarAsync(int carId);
        
        // Helper methods
        Task<List<SelectListItem>> GetCarMakesSelectListAsync();
        Task<bool> IsLicensePlateExistsAsync(string licensePlate, int? excludeCarId = null);
        Task<CarRequest?> GetCarForEditAsync(int carId);
        
        // Validation methods
        Task<(bool CanDelete, string Message)> CanDeleteCarAsync(int carId);
    }
} 
