// File: LearningApplicationService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Linq.Expressions;

public class LearningApplicationService : ILearningApplicationService
{
    private readonly ILearningApplicationRepository _learningApplicationRepository;
    private readonly IMapper _mapper;
    public LearningApplicationService(ILearningApplicationRepository learningApplicationRepository, IMapper mapper)
    {
        _learningApplicationRepository = learningApplicationRepository;
        _mapper = mapper;
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

    public async Task<List<LearningApplicationsResponse>> FindEligibleAsync(byte? licenceTypeId = null)
    {
        var query = (await _learningApplicationRepository.GetAllAsync())
            .Include(x => x.Learner).ThenInclude(x => x.Cccd)
            .Include(x => x.TestApplications)
            .Include(x => x.LicenceType)
            .Where(x => x.TestEligibility == true && !x.TestApplications.Any());

        if (licenceTypeId.HasValue)
        {
            query = query.Where(x => x.LicenceTypeId == licenceTypeId.Value);
        }

        return query.Select(x => ToDto(x, null, null)).ToList();
    }

    public static LearningApplicationsResponse ToDto(LearningApplication la, User? instr = null, List<LearnerClassInfo>? learnerClasses = null)
    {
        // Kiểm tra xem học viên có được gán vào lớp với giảng viên không
        bool hasInstructor = instr != null || (learnerClasses != null && learnerClasses.Any());
        
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
            LearningStatusName = GetLearningStatusName(la.LearningStatus, hasInstructor)
        };
    }

    /// <summary>
    /// Xác định tên trạng thái học dựa trên trạng thái lưu trữ và việc có được gán vào lớp với giảng viên hay không
    /// </summary>
    private static string GetLearningStatusName(byte? learningStatus, bool hasInstructor)
    {
        // Nếu đã có trạng thái cụ thể, ưu tiên trạng thái đó
        if (learningStatus.HasValue)
        {
            return learningStatus.Value switch
            {
                1 => "Đang học",
                2 => "Bảo lưu",
                3 => "Học lại",
                4 => "Hoàn thành",
                _ => hasInstructor ? "Đang học" : "Chưa bắt đầu"
            };
        }

        // Nếu không có trạng thái cụ thể, kiểm tra xem có giảng viên không
        return hasInstructor ? "Đang học" : "Chưa bắt đầu";
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


    public async Task UpdateLearnerProgress(UpdateLearnerProgressRequest request)
    {
        var learningapp = await _learningApplicationRepository.GetByIdAsync(request.LearningId);
        if (learningapp.PracticalDistance == null) learningapp.PracticalDistance = request.PracticalDistance;
        else learningapp.PracticalDistance += request.PracticalDistance;

        if (learningapp.PracticalDurationHours == null) learningapp.PracticalDurationHours = request.PracticalDurationHours;
        else learningapp.PracticalDurationHours += request.PracticalDurationHours;

        await _learningApplicationRepository.UpdateAsync(learningapp);
    }

    public Task<bool> UpdateTestEligibilityAsync(int learningId, bool eligibility)
    {
        return _learningApplicationRepository.UpdateTestEligibilityAsync(learningId, eligibility);
    }



}