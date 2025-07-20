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
    }
}
