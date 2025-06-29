using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Linq.Expressions;

public class LearningApplicationService : ILearningApplicationService
{
    private readonly ILearningApplicationRepository _learningApplicationRepository;
    public LearningApplicationService(ILearningApplicationRepository learningApplicationRepository)
    {
        _learningApplicationRepository = learningApplicationRepository;
    }

    public async Task<List<LearningApplicationsResponse>> FindByCCCD(string cccd, Expression<Func<LearningApplication, bool>>? additional = null)
    {
        var result = (await _learningApplicationRepository
            .GetAllAsync())
            .Include(x => x.Learner)
            .ThenInclude(x => x.Cccd)
            .Include(x => x.TestApplications)
            .Include(x => x.LicenceType)
            .Where(x => x.Learner.Cccd != null && x.Learner.Cccd.CccdNumber.Equals(cccd));
        if (additional != null)
        {
            result = result.Where(additional);
        }

        return [.. result.Select(x => ToDto(x, null, null))];
    }

    public List<LearningApplication> GetAll()
    {
        return _learningApplicationRepository.GetAll();
    }

    public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null)
    {
        return await _learningApplicationRepository.GetAllAsync(searchString);
    }

    public async Task<List<LearnerSummaryResponse>> GetLearnerSummariesAsync(string? searchString = null)
    {
        return await _learningApplicationRepository.GetLearnerSummariesAsync(searchString);
    }

    public async Task<LearningApplicationsResponse?> GetDetailAsync(int id)
    {
        return await _learningApplicationRepository.GetDetailAsync(id);
    }

    public static LearningApplicationsResponse ToDto(LearningApplication la, User? instr = null, List<LearnerClassInfo>? learnerClasses = null)
    {
        return new LearningApplicationsResponse
        {
            LearningId = la.LearningId,
            LearnerId = la.LearnerId,
            LearnerFullName = la.Learner != null
                        ? (la.Learner.LastName ?? "") + " " + (la.Learner.MiddleName ?? "") + " " + (la.Learner.FirstName ?? "")
                        : "",
            LearnerCccdNumber = la.Learner != null && la.Learner.Cccd != null
                        ? la.Learner.Cccd.CccdNumber
                        : "",
            LearnerDob = la.Learner != null && la.Learner.Dob.HasValue
                        ? la.Learner.Dob.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null,
            LearnerPhone = la.Learner != null ? la.Learner.Phone : "",
            LearnerEmail = la.Learner != null ? la.Learner.Email : "",
            LearnerCccdImageUrl = la.Learner != null && la.Learner.Cccd != null
                        ? (la.Learner.Cccd.ImageMt != null ? la.Learner.Cccd.ImageMt : "") + "|" + (la.Learner.Cccd.ImageMs != null ? la.Learner.Cccd.ImageMs : "")
                        : "",
            LearnerHealthCertImageUrl = la.Learner != null && la.Learner.HealthCertificate != null
                        ? la.Learner.HealthCertificate.ImageUrl ?? ""
                        : "",
            LicenceTypeId = la.LicenceTypeId,
            LicenceTypeName = la.LicenceType != null ? la.LicenceType.LicenceCode : "",
            InstructorId = instr != null ? instr.UserId : null,
            InstructorFullName = instr != null
            ? (instr.LastName ?? "") + " " + (instr.MiddleName ?? "") + " " + (instr.FirstName ?? "")
            : "",
            LearnerClasses = learnerClasses ?? new List<LearnerClassInfo>(),
            SubmittedAt = la.SubmittedAt,
            LearningStatus = la.LearningStatus,
            LearningStatusName = la.LearningStatus == 1 ? "Đang học"
                                        : la.LearningStatus == 2 ? "Hoàn thành"
                                        : la.LearningStatus == 3 ? "Đã huỷ"
                                        : "Chưa xác định"
        };
    }
    public async Task AddAsync(LearningApplication entity)
    {
        await _learningApplicationRepository.AddAsync(entity);
    }
}