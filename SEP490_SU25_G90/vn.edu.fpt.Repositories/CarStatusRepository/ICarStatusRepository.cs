using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.CarStatusRepository
{
    public interface ICarStatusRepository
    {
        public Task<IList<CarAssignmentStatus>> GetCarAssignmentStatuses();
    }
}
