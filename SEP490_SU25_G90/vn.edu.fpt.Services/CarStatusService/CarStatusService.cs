using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarStatusRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarStatusService
{
    public class CarStatusService : ICarStatusService
    {
        private readonly ICarStatusRepository _carStatusRepository;

        public CarStatusService(ICarStatusRepository carStatusRepository)
        {
            _carStatusRepository = carStatusRepository;
        }

        public async Task<IList<CarAssignmentStatus>> GetCarAssignmentStatusAsync()
        {
            return await _carStatusRepository.GetCarAssignmentStatuses();
        }
    }
}
