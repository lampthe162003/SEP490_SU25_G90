// File: LearningApplicationService.cs
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

        return result.Select(x => ToDto(x, null, null)).ToList();
    }

    public List<LearningApplication> GetAll()
    {
        return _learningApplicationRepository.GetAll();
    }

    public async Task<List<LearningApplicationsResponse>> GetAllAsync(string? searchString = null, int? statusFilter = null)
    {
        return await _learningApplicationRepository.GetAllAsync(searchString, statusFilter);
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
            LearnerFullName = la.Learner != null ?
                string.Join(" ", new[] { la.Learner.FirstName, la.Learner.MiddleName, la.Learner.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))) : "",
            LearnerCccdNumber = la.Learner?.Cccd?.CccdNumber ?? "",
            LearnerDob = la.Learner?.Dob?.ToDateTime(TimeOnly.MinValue),
            LearnerPhone = la.Learner?.Phone ?? "",
            LearnerEmail = la.Learner?.Email ?? "",
            LearnerCccdImageUrl = la.Learner?.Cccd != null ?
                (la.Learner.Cccd.ImageMt ?? "") + "|" + (la.Learner.Cccd.ImageMs ?? "") : "",
            LearnerHealthCertImageUrl = la.Learner?.HealthCertificate?.ImageUrl ?? "",
            LicenceTypeId = la.LicenceTypeId,
            LicenceTypeName = la.LicenceType?.LicenceCode ?? "",
            LearnerClasses = learnerClasses ?? new List<LearnerClassInfo>(),
            SubmittedAt = la.SubmittedAt,
            LearningStatus = la.LearningStatus,
            LearningStatusName = la.LearningStatus == 1 ? "Đang học" :
                                 la.LearningStatus == 2 ? "Bảo lưu" :
                                 la.LearningStatus == 3 ? "Học lại" :
                                 la.LearningStatus == 4 ? "Hoàn Thành" :
                                 "Chưa xác định"
        };
    }

    public async Task AddAsync(LearningApplication entity)
    {
        await _learningApplicationRepository.AddAsync(entity);
    }

    public async Task<LearningApplicationsResponse?> FindLearnerByCccdAsync(string cccd)
    {
        return await _learningApplicationRepository.FindLearnerByCccdAsync(cccd);
    }

    public Task<bool> UpdateStatusAsync(int learningId, byte newStatus)
    {
        return _learningApplicationRepository.UpdateStatusAsync(learningId, newStatus);
    }

}