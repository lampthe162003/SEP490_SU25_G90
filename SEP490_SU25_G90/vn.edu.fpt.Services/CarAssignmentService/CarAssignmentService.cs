using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public class CarAssignmentService : ICarAssignmentService
    {
        private readonly ICarAssignmentRepository _carAssignmentRepository;

        public CarAssignmentService(ICarAssignmentRepository carAssignmentRepository)
        {
            _carAssignmentRepository = carAssignmentRepository;
        }

        public async Task AddCarAssignment(CarAssignment carAssignment)
        {
            await _carAssignmentRepository.AddCarAssignmentAsync(carAssignment);
        }

        public async Task DeleteCarAssignment(CarAssignment carAssignment)
        {
            if (!await DoesCarAssignmentExist(carAssignment))
            {
                throw new NullReferenceException("Lịch mượn xe không tồn tại.");
            }
            await _carAssignmentRepository.DeleteCarAssignmentAsync(carAssignment);
        }

        public async Task<IList<CarAssignment>> GetAllCarAssignments()
        {
            return await _carAssignmentRepository.GetAllCarAssignmentsAsync();
        }

        public async Task<CarAssignment> GetAssignmentById(int assignmentId)
        {
            return await _carAssignmentRepository.GetAssignmentByIdAsync(assignmentId);
        }

        public async Task<IList<CarAssignment>> GetAssignmentsByCarId(int carId)
        {
            return await _carAssignmentRepository.GetAllAssignmentsByCarIdAsync(carId);
        }

        public async Task UpdateCarAssignment(CarAssignment carAssignment)
        {
            if (!await DoesCarAssignmentExist(carAssignment))
            {
                throw new NullReferenceException("Lịch mượn xe không tồn tại.");
            }
            await _carAssignmentRepository.UpdateCarAssignmentAsync(carAssignment);
        }

        private async Task<bool> DoesCarAssignmentExist(CarAssignment carAssignment)
        {
            return await _carAssignmentRepository.GetAssignmentByIdAsync(carAssignment.CarId) != null;
        }
    }
}
