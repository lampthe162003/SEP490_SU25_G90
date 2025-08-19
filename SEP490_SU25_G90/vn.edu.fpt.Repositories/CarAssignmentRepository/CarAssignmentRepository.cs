using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository
{
    public class CarAssignmentRepository : ICarAssignmentRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public CarAssignmentRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<List<CarAssignment>> GetAllCarAssignmentsAsync()
        {
            return await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                    .ThenInclude(i => i.InstructorSpecializations)
                        .ThenInclude(ins => ins.LicenceType)
                .Include(ca => ca.Slot)
                .Include(ca => ca.CarStatusNavigation)
                .OrderByDescending(ca => ca.ScheduleDate)
                .ThenBy(ca => ca.SlotId)
                .ToListAsync();
        }

        public async Task<List<CarAssignment>> GetAllCarsWithAssignmentsAsync()
        {
            // Lấy tất cả xe từ bảng Cars
            var cars = await _context.Cars.ToListAsync();
            
            // Lấy tất cả assignments
            var allAssignments = await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                    .ThenInclude(i => i.InstructorSpecializations)
                        .ThenInclude(ins => ins.LicenceType)
                .Include(ca => ca.Slot)
                .OrderByDescending(ca => ca.AssignmentId) // Lấy theo ID mới nhất
                .ToListAsync();

            var result = new List<CarAssignment>();

            // Với mỗi xe, tìm assignment mới nhất
            foreach (var car in cars)
            {
                var latestAssignment = allAssignments
                    .Where(a => a.CarId == car.CarId)
                    .OrderByDescending(a => a.AssignmentId) // Record mới nhất
                    .FirstOrDefault();

                if (latestAssignment != null)
                {
                    // Có assignment - hiển thị thông tin từ record mới nhất
                    result.Add(latestAssignment);
                }
                else
                {
                    // Không có assignment - tạo virtual record cho xe trống
                    result.Add(new CarAssignment
                    {
                        AssignmentId = 0,
                        CarId = car.CarId,
                        Car = car,
                        SlotId = 0,
                        Slot = null,
                        ScheduleDate = null,
                        CarStatus = 0, // Xe trống
                        InstructorId = 0,
                        Instructor = null
                    });
                }
            }

            return result.OrderBy(r => r.CarId).ToList();
        }

        public async Task<CarAssignment?> GetCarAssignmentByIdAsync(int assignmentId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                .Include(ca => ca.Slot)
                .Include(ca => ca.CarStatusNavigation)
                .FirstOrDefaultAsync(ca => ca.AssignmentId == assignmentId);
        }

        public async Task<List<CarAssignment>> GetCarAssignmentsByInstructorAsync(int instructorId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                .Include(ca => ca.Slot)
                .Where(ca => ca.InstructorId == instructorId)
                .OrderByDescending(ca => ca.ScheduleDate)
                .ToListAsync();
        }

        public async Task<List<CarAssignment>> GetCarAssignmentsByDateAsync(DateOnly date)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                    .ThenInclude(i => i.InstructorSpecializations)
                        .ThenInclude(ins => ins.LicenceType)
                .Include(ca => ca.Slot)
                .Where(ca => ca.ScheduleDate == date)
                .OrderBy(ca => ca.SlotId)
                .ToListAsync();
        }

        public async Task<List<CarAssignment>> GetCarAssignmentsByLicenceTypeAsync(byte licenceTypeId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                    .ThenInclude(i => i.InstructorSpecializations)
                        .ThenInclude(ins => ins.LicenceType)
                .Include(ca => ca.Slot)
                .Where(ca => ca.Instructor.InstructorSpecializations
                    .Any(ins => ins.LicenceTypeId == licenceTypeId))
                .OrderByDescending(ca => ca.ScheduleDate)
                .ToListAsync();
        }

        public async Task<List<CarAssignment>> SearchCarAssignmentsAsync(string? carMake, DateOnly? scheduleDate)
        {
            // Lấy tất cả xe và filter theo carMake nếu có
            var carsQuery = _context.Cars.AsQueryable();
            if (!string.IsNullOrEmpty(carMake))
            {
                carsQuery = carsQuery.Where(c => c.CarMake.Contains(carMake));
            }
            var cars = await carsQuery.ToListAsync();

            // Lấy tất cả assignments
            var allAssignments = await _context.CarAssignments
                .Include(ca => ca.Car)
                .Include(ca => ca.Instructor)
                    .ThenInclude(i => i.InstructorSpecializations)
                        .ThenInclude(ins => ins.LicenceType)
                .Include(ca => ca.Slot)
                .OrderByDescending(ca => ca.AssignmentId)
                .ToListAsync();

            var result = new List<CarAssignment>();

            // Với mỗi xe đã filter, tìm assignment mới nhất
            foreach (var car in cars)
            {
                var latestAssignment = allAssignments
                    .Where(a => a.CarId == car.CarId)
                    .OrderByDescending(a => a.AssignmentId)
                    .FirstOrDefault();

                // Filter theo scheduleDate nếu có
                if (scheduleDate.HasValue && latestAssignment != null)
                {
                    if (latestAssignment.ScheduleDate != scheduleDate.Value)
                    {
                        continue; // Skip xe này nếu không match date
                    }
                }

                if (latestAssignment != null)
                {
                    result.Add(latestAssignment);
                }
                else
                {
                    // Xe trống
                    result.Add(new CarAssignment
                    {
                        AssignmentId = 0,
                        CarId = car.CarId,
                        Car = car,
                        SlotId = 0,
                        Slot = null,
                        ScheduleDate = null,
                        CarStatus = 0,
                        InstructorId = 0,
                        Instructor = null
                    });
                }
            }

            return result.OrderBy(r => r.CarId).ToList();
        }

        public async Task<CarAssignment> CreateCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Add(carAssignment);
            await _context.SaveChangesAsync();
            return carAssignment;
        }

        public async Task<bool> UpdateCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Update(carAssignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCarAssignmentAsync(int assignmentId)
        {
            var carAssignment = await _context.CarAssignments.FindAsync(assignmentId);
            if (carAssignment == null)
                return false;

            _context.CarAssignments.Remove(carAssignment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Car>> GetAvailableCarsAsync(DateOnly date, int slotId)
        {
            var assignedCarIds = await _context.CarAssignments
                .Where(ca => ca.ScheduleDate == date && ca.SlotId == slotId)
                .Select(ca => ca.CarId)
                .ToListAsync();

            return await _context.Cars
                .Where(c => !assignedCarIds.Contains(c.CarId))
                .ToListAsync();
        }

        public async Task<List<ScheduleSlot>> GetAllScheduleSlotsAsync()
        {
            return await _context.ScheduleSlots
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<LicenceType>> GetAllLicenceTypesAsync()
        {
            return await _context.LicenceTypes.ToListAsync();
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateOnly date, int slotId)
        {
            return !await _context.CarAssignments
                .AnyAsync(ca => ca.CarId == carId && ca.ScheduleDate == date && ca.SlotId == slotId);
        }

        public async Task AddCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Add(carAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAssignmentAsync(CarAssignment carAssignment)
        {
            _context.CarAssignments.Remove(carAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<CarAssignment>> GetAllAssignmentsByCarIdAsync(int carId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Instructor)
                .Include(ca => ca.Slot)
                .Include(ca => ca.CarStatusNavigation)
                .Where(ca => ca.CarId == carId)
                .ToListAsync();
        }

        public async Task<CarAssignment> GetAssignmentByIdAsync(int assignmentId)
        {
            return await _context.CarAssignments
                .Include(ca => ca.Instructor)
                .Where(ca => ca.AssignmentId == assignmentId)
                .FirstOrDefaultAsync();
        }
    }
}

