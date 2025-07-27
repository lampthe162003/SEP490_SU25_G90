using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.InstructorService
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;

        public InstructorService(IInstructorRepository instructorRepository, IMapper mapper)
        {
            _instructorRepository = instructorRepository;
            _mapper = mapper;
        }

        public IList<InstructorListInformationResponse> GetAllInstructors(string? name = null, byte? licenceTypeId = null)
        {
            var query = _instructorRepository.GetAllInstructors();

            if (!string.IsNullOrEmpty(name))
            {
                query = _instructorRepository.GetInstructorsByName(query, name);
            }

            if (licenceTypeId.HasValue)
            {
                query = _instructorRepository.GetInstructorsByLicenceType(query, licenceTypeId.Value);
            }

            var instructors = query.ToList();

            var result = instructors.Select(instructor => new InstructorListInformationResponse
            {
                UserId = instructor.UserId,
                Email = instructor.Email,
                FirstName = instructor.FirstName,
                MiddleName = instructor.MiddleName,
                LastName = instructor.LastName,
                Dob = instructor.Dob,
                Gender = instructor.Gender,
                Phone = instructor.Phone,
                ProfileImageUrl = instructor.ProfileImageUrl,
                CccdNumber = instructor.Cccd?.CccdNumber,
                CccdImageUrl = instructor.Cccd?.ImageMt,
                CccdImageUrlMs = instructor.Cccd?.ImageMs,
                AddressDisplay = BuildAddressDisplay(instructor.Address),
                Specializations = instructor.InstructorSpecializations.Select(ins => new LicenceTypeResponse
                {
                    LicenceTypeId = ins.LicenceType.LicenceTypeId,
                    LicenceCode = ins.LicenceType.LicenceCode
                }).ToList(),
                //StudentCount = instructor.LearningApplicationInstructors.Count(la => la.LearningStatus == 1) // Active learning applications
            }).ToList();

            return result;
        }

        public InstructorListInformationResponse? GetInstructorById(int id)
        {
            var instructor = _instructorRepository.GetInstructorById(id);
            if (instructor == null) return null;

            return new InstructorListInformationResponse
            {
                UserId = instructor.UserId,
                Email = instructor.Email,
                FirstName = instructor.FirstName,
                MiddleName = instructor.MiddleName,
                LastName = instructor.LastName,
                Dob = instructor.Dob,
                Gender = instructor.Gender,
                Phone = instructor.Phone,
                ProfileImageUrl = instructor.ProfileImageUrl,
                CccdNumber = instructor.Cccd?.CccdNumber,
                CccdImageUrl = instructor.Cccd?.ImageMt,
                CccdImageUrlMs = instructor.Cccd?.ImageMs,
                AddressDisplay = BuildAddressDisplay(instructor.Address),
                Specializations = instructor.InstructorSpecializations.Select(ins => new LicenceTypeResponse
                {
                    LicenceTypeId = ins.LicenceType.LicenceTypeId,
                    LicenceCode = ins.LicenceType.LicenceCode
                }).ToList(),
                //StudentCount = instructor.LearningApplicationInstructors.Count(la => la.LearningStatus == 1)
            };
        }

        public void CreateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor)
        {
            _instructorRepository.Create(instructor);
        }

        public void UpdateInstructor(SEP490_SU25_G90.vn.edu.fpt.Models.User instructor)
        {
            _instructorRepository.Update(instructor);
        }

        public void UpdateInstructorInfo(int instructorId, UpdateInstructorRequest request)
        {
            _instructorRepository.UpdateInstructorInfo(instructorId, request);
        }

        public void DeleteInstructor(int id)
        {
            _instructorRepository.Delete(id);
        }

        public void AddSpecialization(int instructorId, byte licenceTypeId)
        {
            var specialization = new InstructorSpecialization
            {
                InstructorId = instructorId,
                LicenceTypeId = licenceTypeId
            };
            _instructorRepository.AddSpecialization(specialization);
        }

        public void RemoveSpecialization(int instructorId, byte licenceTypeId)
        {
            _instructorRepository.RemoveSpecialization(instructorId, licenceTypeId);
        }

        public List<LicenceTypeResponse> GetAllLicenceTypes()
        {
            var licenceTypes = _instructorRepository.GetAllLicenceTypes();
            return licenceTypes.Select(lt => new LicenceTypeResponse
            {
                LicenceTypeId = lt.LicenceTypeId,
                LicenceCode = lt.LicenceCode
            }).ToList();
        }

        public async Task<List<LearnerUserResponse>> GetAllLearnersAsync(string? searchString = null)
        {
            return await _instructorRepository.GetAllLearnersAsync(searchString);
        }

        private string BuildAddressDisplay(Address? address)
        {
            if (address == null) return "";

            var parts = new List<string>();

            if (!string.IsNullOrEmpty(address.HouseNumber))
                parts.Add(address.HouseNumber);

            if (!string.IsNullOrEmpty(address.RoadName))
                parts.Add(address.RoadName);

            if (address.Ward != null)
            {
                if (!string.IsNullOrEmpty(address.Ward.WardName))
                    parts.Add(address.Ward.WardName);

                if (address.Ward.Province != null)
                {
                    if (!string.IsNullOrEmpty(address.Ward.Province.ProvinceName))
                        parts.Add(address.Ward.Province.ProvinceName);

                    if (address.Ward.Province.City != null && !string.IsNullOrEmpty(address.Ward.Province.City.CityName))
                        parts.Add(address.Ward.Province.City.CityName);
                }
            }

            return string.Join(", ", parts);
        }

        public async Task<bool> UpdateLearnerScoresAsync(int learningId, int? theory, int? simulation, int? obstacle, int? practical)
        {
            return await _instructorRepository.UpdateLearnerScoresAsync(learningId, theory, simulation, obstacle, practical);
        }
        public async Task<List<LearningApplicationsResponse>> GetLearningApplicationsByInstructorAsync(int instructorId)
        {
            return await _instructorRepository.GetLearningApplicationsByInstructorAsync(instructorId);
        }
        public async Task<LearningApplicationsResponse?> GetLearningApplicationDetailAsync(int learningId)
        {
            return await _instructorRepository.GetLearningApplicationDetailAsync(learningId);
        }

    }
}