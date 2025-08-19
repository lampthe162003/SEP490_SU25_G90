using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.CarStatusService
{
    public interface ICarStatusService
    {
        public Task<IList<CarAssignmentStatus>> GetCarAssignmentStatusAsync();
    }
}
