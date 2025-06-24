using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;

public class LearningApplicationRepository : ILearningApplicationRepository
{
    private readonly Sep490Su25G90DbContext _context;

    public LearningApplicationRepository(Sep490Su25G90DbContext context)
    {
        _context = context;
    }

    public List<LearningApplication> GetAll()
    {
        return _context.LearningApplications
            .Include(la => la.Learner)
            .Include(la => la.LicenceType)
            .Include(la => la.Instructor)
            .ToList();
    }

    public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null)
    {
        var query = _context.LearningApplications
            .Include(x => x.Learner).ThenInclude(l => l.Cccd)
            .Include(x => x.LicenceType)
            .Include(x => x.Instructor)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            query = query.Where(x =>
                ((
                    (x.Learner != null ? (x.Learner.LastName ?? "") + " " + (x.Learner.MiddleName ?? "") + " " + (x.Learner.FirstName ?? "") : "")
                ).Contains(searchString)) ||
                ((x.Learner != null && x.Learner.Cccd != null ? x.Learner.Cccd.CccdNumber ?? "" : "").Contains(searchString)) ||
                ((x.LicenceType != null ? x.LicenceType.LicenceCode ?? "" : "").Contains(searchString))
            );
        }

        return await query
            .Select(x => new LearningApplicationsResponse
            {
                LearningId = x.LearningId,
                LearnerId = x.LearnerId,
                LearnerFullName = x.Learner != null
                    ? (x.Learner.LastName ?? "") + " " + (x.Learner.MiddleName ?? "") + " " + (x.Learner.FirstName ?? "")
                    : "",
                LearnerCccdNumber = x.Learner != null && x.Learner.Cccd != null
                    ? x.Learner.Cccd.CccdNumber
                    : "",
                LearnerDob = x.Learner != null && x.Learner.Dob.HasValue ? x.Learner.Dob.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                LearnerPhone = x.Learner != null ? x.Learner.Phone : "",
                LearnerEmail = x.Learner != null ? x.Learner.Email : "",
                LicenceTypeId = x.LicenceTypeId,
                LicenceTypeName = x.LicenceType != null ? x.LicenceType.LicenceCode : "",
                InstructorId = x.InstructorId,
                InstructorFullName = x.Instructor != null
                    ? (x.Instructor.LastName ?? "") + " " + (x.Instructor.MiddleName ?? "") + " " + (x.Instructor.FirstName ?? "")
                    : "",
                SubmittedAt = x.SubmittedAt,
                LearningStatus = x.LearningStatus,
                LearningStatusName = x.LearningStatus == 1 ? "Đang học" :
                                    x.LearningStatus == 2 ? "Hoàn thành" :
                                    x.LearningStatus == 3 ? "Đã huỷ" : "Chưa xác định"
            })
            .ToListAsync();
    }

    // Thêm phương thức lấy chi tiết
    public async Task<LearningApplicationsResponse?> GetDetailAsync(int id)
    {
        return await _context.LearningApplications
            .Include(x => x.Learner).ThenInclude(l => l.Cccd)
            .Include(x => x.Learner).ThenInclude(l => l.HealthCertificate)
            .Include(x => x.LicenceType)
            .Include(x => x.Instructor)
            .Where(x => x.LearningId == id)
            .Select(x => new LearningApplicationsResponse
            {
                LearningId = x.LearningId,
                LearnerId = x.LearnerId,
                LearnerFullName = x.Learner != null
                    ? (x.Learner.LastName ?? "") + " " + (x.Learner.MiddleName ?? "") + " " + (x.Learner.FirstName ?? "")
                    : "",
                LearnerCccdNumber = x.Learner != null && x.Learner.Cccd != null
                    ? x.Learner.Cccd.CccdNumber
                    : "",
                LearnerDob = x.Learner != null && x.Learner.Dob.HasValue ? x.Learner.Dob.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                LearnerPhone = x.Learner != null ? x.Learner.Phone : "",
                LearnerEmail = x.Learner != null ? x.Learner.Email : "",
                LicenceTypeId = x.LicenceTypeId,
                LicenceTypeName = x.LicenceType != null ? x.LicenceType.LicenceCode : "",
                InstructorId = x.InstructorId,
                InstructorFullName = x.Instructor != null
                    ? (x.Instructor.LastName ?? "") + " " + (x.Instructor.MiddleName ?? "") + " " + (x.Instructor.FirstName ?? "")
                    : "",
                SubmittedAt = x.SubmittedAt,
                LearningStatus = x.LearningStatus,
                LearningStatusName = x.LearningStatus == 1 ? "Đang học" :
                                    x.LearningStatus == 2 ? "Hoàn thành" :
                                    x.LearningStatus == 3 ? "Đã huỷ" : "Chưa xác định"
            })
            .FirstOrDefaultAsync();
    }
}