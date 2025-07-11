using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.TestApplicationRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.TestApplication
{
    public class TestApplicationService : ITestApplicationService
    {
        private readonly ITestApplicationRepository _testApplicationRepository;
        private readonly IMapper _mapper;

        public TestApplicationService(ITestApplicationRepository testApplicationRepository, IMapper mapper)
        {
            _testApplicationRepository = testApplicationRepository;
            _mapper = mapper;
        }

        public async Task<Models.TestApplication> CreateTestApplication(CreatUpdateTestApplicationRequest request)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "test-application");
            Directory.CreateDirectory(path);
            if (request.Attachment != null)
            {
                path = Path.Combine(path, request.Attachment.FileName);
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                {
                    await request.Attachment.CopyToAsync(fs);
                }
            }
            Models.TestApplication testApplication = new Models.TestApplication()
            {
                ExamDate = request.ExamDate,
                LearningId = request.LearningApplicationId,
                Notes = request.Note,
                ObstacleScore = request.ObstacleScore,
                PracticalScore = request.PracticalScore,
                SimulationScore = request.SimulationScore,
                TheoryScore = request.TheoryScore,
                Status = null,
                ResultImageUrl = request.Attachment == null ? null : path,
                SubmitProfileDate = request.SubmitProfileDate.HasValue ? request.SubmitProfileDate.Value : null,
            };
            return await _testApplicationRepository.Create(testApplication);
        }

        public async Task<List<TestApplicationListInformationResponse>> GetAllTestApplicationAsync()
        {
            var list = await _testApplicationRepository.GetAllTestApplicationAsync();
            return _mapper.Map<List<TestApplicationListInformationResponse>>(list);
        }

        public async Task<Pagination<TestApplicationListInformationResponse>> SearchAll(TestApplicationSearchRequest request)
        {
            var dataSrc = _testApplicationRepository.GetAll();
            var data = dataSrc
               .Include(t => t.Learning)
                   .ThenInclude(la => la.Learner)
                       .ThenInclude(u => u.Cccd)
               .Include(t => t.Learning)
               .AsQueryable();
            if (request.LicenseTypeId.HasValue)
            {
                data = data.Where(x => x.Learning.LicenceTypeId == request.LicenseTypeId.Value);
            }

            if (request.Status.HasValue)
            {
                if (request.Status == 0)
                {
                    data = data.Where(x => x.Status == null);
                }
                else
                if (request.Status == 1)
                {
                    data = data.Where(x => x.Status == true);
                }
                else
                {
                    data = data.Where(x => x.Status == false);
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var normallizedName = request.Search.Trim();
                data = data.Where(t => t.Learning.Learner.FirstName.Contains(normallizedName) ||
                            t.Learning.Learner.MiddleName.Contains(normallizedName) ||
                            t.Learning.Learner.LastName.Contains(normallizedName) ||
                            t.Learning.Learner.Cccd.CccdNumber.Contains(normallizedName)
                            );
            }

            var totalCount = await data.CountAsync();
            var paginatedData = await data
                .Skip(request.Skip)
                .Take(TestApplicationSearchRequest.Take)
                .ToListAsync();
            var mappedData = _mapper.Map<List<TestApplicationListInformationResponse>>(paginatedData);
            return new Pagination<TestApplicationListInformationResponse>()
            {
                Data = mappedData,
                Total = totalCount,
                TotalPage = totalCount % TestApplicationSearchRequest.Take == 0
                ? totalCount / TestApplicationSearchRequest.Take
                : ((totalCount / TestApplicationSearchRequest.Take) + 1)
            };
        }

        public async Task<CreatUpdateTestApplicationRequest> FindById(int id)
        {
            var obj = await _testApplicationRepository
                .GetAll()
                .Include(x => x.Learning)
                .ThenInclude(x => x.Learner)
                .ThenInclude(x => x.Cccd)
                .Include(x => x.Learning)
                .ThenInclude(x => x.Learner)
                .ThenInclude(x => x.Address.Ward.Province.City)
                .Include(x => x.Learning)
                .ThenInclude(x => x.LicenceType)
                .FirstOrDefaultAsync(x => x.TestId == id);
            return new CreatUpdateTestApplicationRequest()
            {
                CCCD = obj.Learning.Learner.Cccd?.CccdNumber,
                ExamDate = obj.ExamDate,
                LearningApplicationId = id,
                Note = obj.Notes,
                ObstacleScore = obj.ObstacleScore,
                PracticalScore = obj.PracticalScore,
                SimulationScore = obj.SimulationScore,
                TheoryScore = obj.TheoryScore,
                SubmitProfileDate = obj.SubmitProfileDate.HasValue ? obj.SubmitProfileDate.Value : null,
                DateOfBirth = obj.Learning.Learner.Dob.HasValue ? obj.Learning.Learner.Dob.Value.ToString("dd-MM-yyyy") : null,
                FullName = obj.Learning == null || obj.Learning.Learner == null
                    ? string.Empty
                    : $"{obj.Learning.Learner.FirstName} {obj.Learning.Learner.MiddleName} {obj.Learning.Learner.LastName}",
                LicenseType = obj.Learning?.LicenceType?.LicenceCode ?? string.Empty,
                ResultImageUrl = obj.ResultImageUrl,
                Email = obj.Learning.Learner?.Email,
                Phone = obj.Learning.Learner?.Phone,
                Address = string.Join(",", (new List<string>() {  obj.Learning.Learner?.Address?.HouseNumber,
obj.Learning.Learner?.Address?.RoadName, obj.Learning.Learner?.Address?.Ward.WardName, obj.Learning.Learner?.Address?.Ward.Province.ProvinceName,
obj.Learning.Learner?.Address?.Ward.Province.City.CityName}).Where(x => !string.IsNullOrWhiteSpace(x))),
                Gender = obj.Learning.Learner.Gender == true ? "Nam" : "Nữ",
                Status = obj.Status,
                FileUrl = obj.ResultImageUrl
            };
        }

        public async Task<Models.TestApplication> UpdateTestApplication(int id, CreatUpdateTestApplicationRequest request, bool? status)
        {
            var raw = await _testApplicationRepository
                .GetAll()
                .FirstOrDefaultAsync(x => x.TestId == id);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "test-application");
            if (request.Attachment != null)
            {
                if (!string.IsNullOrEmpty(raw.ResultImageUrl))
                {
                    File.Delete(raw.ResultImageUrl);
                }

                Directory.CreateDirectory(path);
                path = Path.Combine(path, request.Attachment.FileName);
                using (FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                {
                    await request.Attachment.CopyToAsync(fs);
                }
            }

            raw.ResultImageUrl = request.Attachment == null ? raw.ResultImageUrl : path;
            raw.Notes = request.Note;
            raw.SubmitProfileDate = request.SubmitProfileDate.HasValue ? request.SubmitProfileDate.Value : null;
            raw.Notes = request.Note;
            raw.ObstacleScore = request.ObstacleScore;
            raw.PracticalScore = request.PracticalScore;
            raw.SimulationScore = request.SimulationScore;
            raw.TheoryScore = request.TheoryScore;
            raw.ExamDate = request.ExamDate;
            raw.Status = status.HasValue ? status : raw.Status;
            return await _testApplicationRepository.Update(raw);

        }
    }
}
