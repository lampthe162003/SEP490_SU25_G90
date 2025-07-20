using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarService
{
    public interface ICarService
    {
        public Task<IList<Car>> GetAllCars();
        public Task<Car> GetCarById(int carId);
        public Task AddCar(Car car);
        public Task UpdateCar(Car car);
    }
}
