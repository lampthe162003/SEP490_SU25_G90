using Microsoft.AspNetCore.Mvc.Rendering;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarService
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task AddCar(Car car)
        {
            await _carRepository.AddCarAsync(car);
        }

        public async Task<IList<Car>> GetAllCars()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        public async Task<Car> GetCarById(int carId)
        {
            return await _carRepository.GetCarByIdAsync(carId);
        }

        public async Task UpdateCar(Car car)
        {
            await _carRepository.UpdateCarAsync(car);
        }
        
        public async Task<List<CarResponse>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            var responses = new List<CarResponse>();

            foreach (var car in cars)
            {
                var response = await MapToResponse(car);
                responses.Add(response);
            }

            return responses;
        }

        public async Task<List<CarResponse>> SearchCarsAsync(CarSearchRequest request)
        {
            // Nếu không có điều kiện tìm kiếm nào, trả về tất cả xe
            if (string.IsNullOrWhiteSpace(request.CarMake) && 
                string.IsNullOrWhiteSpace(request.CarModel) && 
                string.IsNullOrWhiteSpace(request.LicensePlate) && 
                !request.IsRented.HasValue)
            {
                return await GetAllCarsAsync();
            }

            var cars = await _carRepository.SearchCarsAsync(
                request.CarMake, 
                request.CarModel, 
                request.LicensePlate);

            var responses = new List<CarResponse>();

            if (cars != null)
            {
                foreach (var car in cars)
                {
                    var response = await MapToResponse(car);

                    // Filter by rental status if specified
                    if (request.IsRented.HasValue)
                    {
                        if (request.IsRented.Value && !response.IsCurrentlyRented)
                            continue;
                        if (!request.IsRented.Value && response.IsCurrentlyRented)
                            continue;
                    }

                    responses.Add(response);
                }
            }

            return responses;
        }

        public async Task<CarResponse?> GetCarByIdAsync(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
                return null;

            return await MapToResponse(car);
        }

        public async Task<bool> CreateCarAsync(CarRequest request)
        {
            // Check if license plate already exists
            if (await _carRepository.IsLicensePlateExistsAsync(request.LicensePlate))
            {
                return false;
            }

            var car = new Car
            {
                CarMake = request.CarMake,
                CarModel = request.CarModel,
                LicensePlate = request.LicensePlate
            };

            var result = await _carRepository.CreateCarAsync(car);
            return result != null;
        }

        public async Task<bool> UpdateCarAsync(int carId, CarRequest request)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
                return false;

            // Check if license plate already exists (excluding current car)
            if (await _carRepository.IsLicensePlateExistsAsync(request.LicensePlate, carId))
            {
                return false;
            }

            car.CarMake = request.CarMake;
            car.CarModel = request.CarModel;
            car.LicensePlate = request.LicensePlate;

            return await _carRepository.UpdateCarAsync(car);
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            // Check if car can be deleted
            var (canDelete, _) = await CanDeleteCarAsync(carId);
            if (!canDelete)
                return false;

            // Delete all car assignments first
            await _carRepository.DeleteCarAssignmentsAsync(carId);

            // Then delete the car
            return await _carRepository.DeleteCarAsync(carId);
        }

        public async Task<List<SelectListItem>> GetCarMakesSelectListAsync()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            var carMakes = cars.Where(c => !string.IsNullOrEmpty(c.CarMake))
                              .Select(c => c.CarMake)
                              .Distinct()
                              .OrderBy(make => make)
                              .ToList();

            return carMakes.Select(make => new SelectListItem
            {
                Value = make,
                Text = make
            }).ToList();
        }

        public async Task<bool> IsLicensePlateExistsAsync(string licensePlate, int? excludeCarId = null)
        {
            return await _carRepository.IsLicensePlateExistsAsync(licensePlate, excludeCarId);
        }

        public async Task<CarRequest?> GetCarForEditAsync(int carId)
        {
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
                return null;

            return new CarRequest
            {
                CarMake = car.CarMake ?? string.Empty,
                CarModel = car.CarModel ?? string.Empty,
                LicensePlate = car.LicensePlate ?? string.Empty
            };
        }

        public async Task<(bool CanDelete, string Message)> CanDeleteCarAsync(int carId)
        {
            var assignmentCount = await _carRepository.GetCarAssignmentCountAsync(carId);
            var activeAssignments = await _carRepository.GetActiveCarAssignmentsAsync(carId);

            if (activeAssignments.Any())
            {
                return (false, $"Không thể xóa xe này vì đang có {activeAssignments.Count} lượt thuê đang hoạt động.");
            }

            if (assignmentCount > 0)
            {
                return (true, $"Xe này có {assignmentCount} lịch sử thuê. Việc xóa xe sẽ xóa tất cả lịch sử thuê tương ứng.");
            }

            return (true, "Xe có thể xóa an toàn.");
        }

        private async Task<CarResponse> MapToResponse(Car car)
        {
            var totalAssignments = await _carRepository.GetCarAssignmentCountAsync(car.CarId);
            var activeAssignments = await _carRepository.GetActiveCarAssignmentsAsync(car.CarId);

            var response = new CarResponse
            {
                CarId = car.CarId,
                CarMake = car.CarMake,
                CarModel = car.CarModel,
                LicensePlate = car.LicensePlate,
                TotalAssignments = totalAssignments,
                ActiveAssignments = activeAssignments.Count,
                IsCurrentlyRented = activeAssignments.Any(),
                CurrentInstructorName = activeAssignments.FirstOrDefault()?.Instructor != null
                    ? GetInstructorFullName(activeAssignments.First().Instructor)
                    : null
            };

            return response;
        }

        private string GetInstructorFullName(Models.User instructor)
        {
            if (instructor == null) return string.Empty;

            var fullName = $"{instructor.FirstName}";
            if (!string.IsNullOrEmpty(instructor.MiddleName))
                fullName += $" {instructor.MiddleName}";
            if (!string.IsNullOrEmpty(instructor.LastName))
                fullName += $" {instructor.LastName}";

            return fullName.Trim();
        }
    }
} 
