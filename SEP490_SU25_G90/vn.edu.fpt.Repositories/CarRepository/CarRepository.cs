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

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars
                .OrderBy(c => c.CarMake)
                .ThenBy(c => c.CarModel)
                .ToListAsync();
        }

        public async Task<Car?> GetCarByIdAsync(int carId)
        {
            return await _context.Cars
                .FirstOrDefaultAsync(c => c.CarId == carId);
        }

        public async Task<List<Car>> SearchCarsAsync(string? carMake, string? carModel, string? licensePlate)
        {
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(carMake))
            {
                query = query.Where(c => c.CarMake != null && c.CarMake.Contains(carMake.Trim()));
            }

            if (!string.IsNullOrEmpty(carModel))
            {
                query = query.Where(c => c.CarModel != null && c.CarModel.Contains(carModel.Trim()));
            }

            if (!string.IsNullOrEmpty(licensePlate))
            {
                query = query.Where(c => c.LicensePlate != null && c.LicensePlate.Contains(licensePlate.Trim()));
            }

            return await query
                .OrderBy(c => c.CarMake)
                .ThenBy(c => c.CarModel)
                .ToListAsync();
        }

        public async Task<Car> CreateCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<bool> UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCarAsync(int carId)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null)
                return false;

            _context.Cars.Remove(car);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> IsLicensePlateExistsAsync(string licensePlate, int? excludeCarId = null)
        {
            var query = _context.Cars.Where(c => c.LicensePlate == licensePlate);
            
            if (excludeCarId.HasValue)
            {
                query = query.Where(c => c.CarId != excludeCarId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<int> GetCarAssignmentCountAsync(int carId)
        {
            return await _context.CarAssignments
                .Where(ca => ca.CarId == carId)
                .CountAsync();
        }

        public async Task<bool> DeleteCarAssignmentsAsync(int carId)
        {
            var assignments = await _context.CarAssignments
                .Where(ca => ca.CarId == carId)
                .ToListAsync();

            if (assignments.Any())
            {
                _context.CarAssignments.RemoveRange(assignments);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<List<CarAssignment>> GetActiveCarAssignmentsAsync(int carId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Instructor)
                .Include(ca => ca.Slot)
                .Where(ca => ca.CarId == carId && ca.CarStatus > 0)
                .ToListAsync();
        }
    }
} 
