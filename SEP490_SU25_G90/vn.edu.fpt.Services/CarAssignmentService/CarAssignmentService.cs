using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.CarAssignment;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarAssignmentRepository;
using System.Threading.Tasks;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarAssignmentService
{
    public class CarAssignmentService : ICarAssignmentService
    {
        private readonly ICarAssignmentRepository _carAssignmentRepository;
        private readonly IMapper _mapper;

        public CarAssignmentService(ICarAssignmentRepository carAssignmentRepository, IMapper mapper)
        {
            _carAssignmentRepository = carAssignmentRepository;
            _mapper = mapper;
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

        public async Task<IList<CarAssignmentInformationResponse>> GetAllCarAssignments()
        {
            var assignments = await _carAssignmentRepository.GetAllCarAssignmentsAsync();
            foreach (var carAssignment in assignments)
            {

            }
            return _mapper.Map<IList<CarAssignmentInformationResponse>>(await _carAssignmentRepository.GetAllCarAssignmentsAsync());
        }

        public async Task<CarAssignmentInformationResponse> GetAssignmentById(int assignmentId)
        {
            return _mapper.Map<CarAssignmentInformationResponse>(await _carAssignmentRepository.GetAssignmentByIdAsync(assignmentId));
        }

        public async Task<IList<CarAssignmentInformationResponse>> GetAssignmentsByCarId(int carId)
        {
            return _mapper.Map<IList<CarAssignmentInformationResponse>>(await _carAssignmentRepository.GetAllAssignmentsByCarIdAsync(carId));
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
