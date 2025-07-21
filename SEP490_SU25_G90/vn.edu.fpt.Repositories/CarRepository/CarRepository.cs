using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository
{
    public class CarRepository : ICarRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public CarRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int carId)
        {
            return await _context.Cars.FindAsync(carId);
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }
    }
}
