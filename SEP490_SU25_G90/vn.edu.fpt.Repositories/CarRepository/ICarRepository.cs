using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository
{
    public interface ICarRepository
    {
        public Task<IList<Car>> GetAllCarsAsync();
        public Task<Car> GetCarByIdAsync(int carId);
        public Task AddCarAsync(Car car);
        public Task UpdateCarAsync(Car car);
    }
}
