using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using System.Linq;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.InstructorRepository
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public InstructorRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllInstructors()
        {
            return _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Include(u => u.Cccd)
                .Include(u => u.UserRoles)
                .Include(u => u.InstructorSpecializations) // Giữ lại để không ảnh hưởng các tính năng khác
                    .ThenInclude(ins => ins.LicenceType)
                //.Include(u => u.LearningApplicationInstructors)
                //.ThenInclude(la => la.Learner)
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == 3)) // Role Instructor
                .AsQueryable();
        }


        public IQueryable<User> GetInstructorsByName(IQueryable<User> query, string name)
        {
            return query.Where(u => 
                u.FirstName.Contains(name) ||
                u.MiddleName.Contains(name) ||
                u.LastName.Contains(name) ||
                (u.FirstName + " " + u.MiddleName + " " + u.LastName).Contains(name));
        }

        public IQueryable<User> GetInstructorsByLicenceType(IQueryable<User> query, byte licenceTypeId)
        {
            return query.Where(u => u.InstructorSpecializations.Any(ins => ins.LicenceTypeId == licenceTypeId));
        }

        public User? GetInstructorById(int id)
        {
            return _context.Users
                .Include(u => u.Address)
                    .ThenInclude(a => a.Ward)
                        .ThenInclude(w => w.Province)
                            .ThenInclude(p => p.City)
                .Include(u => u.Cccd)
                .Include(u => u.InstructorSpecializations)
                    .ThenInclude(ins => ins.LicenceType)
                //.Include(u => u.LearningApplicationInstructors)
                .FirstOrDefault(u => u.UserId == id);
        }

        public void Create(User instructor)
        {
            _context.Users.Add(instructor);
            _context.SaveChanges();
        }

        public void Update(User instructor)
        {
            _context.Users.Update(instructor);
            _context.SaveChanges();
        }

        public void UpdateInstructorInfo(int instructorId, UpdateInstructorRequest request)
        {
            var instructor = _context.Users
                .Include(u => u.Cccd)
                .FirstOrDefault(u => u.UserId == instructorId);
            
            if (instructor != null)
            {
                instructor.FirstName = request.FirstName;
                instructor.MiddleName = request.MiddleName;
                instructor.LastName = request.LastName;
                instructor.Dob = request.Dob;
                instructor.Gender = request.Gender;
                instructor.Phone = request.Phone;

                // Update CCCD if any CCCD field is provided
                if (!string.IsNullOrWhiteSpace(request.CccdNumber) || 
                    !string.IsNullOrWhiteSpace(request.CccdImageFront) || 
                    !string.IsNullOrWhiteSpace(request.CccdImageBack))
                {
                    // Validate CCCD number format if provided
                    if (!string.IsNullOrWhiteSpace(request.CccdNumber))
                    {
                        if (request.CccdNumber.Length != 12 || !request.CccdNumber.All(char.IsDigit))
                        {
                            throw new ArgumentException("Số CCCD phải có đúng 12 chữ số");
                        }
                    }

                    if (instructor.Cccd == null)
                    {
                        // Create new CCCD
                        var newCccd = new Cccd
                        {
                            CccdNumber = request.CccdNumber ?? "",
                            ImageMt = request.CccdImageFront,
                            ImageMs = request.CccdImageBack
                        };
                        _context.Cccds.Add(newCccd);
                        _context.SaveChanges(); // Save to get CccdId
                        instructor.CccdId = newCccd.CccdId;
                    }
                    else
                    {
                        // Update existing CCCD
                        if (!string.IsNullOrWhiteSpace(request.CccdNumber))
                        {
                            instructor.Cccd.CccdNumber = request.CccdNumber;
                        }
                        if (!string.IsNullOrWhiteSpace(request.CccdImageFront))
                        {
                            instructor.Cccd.ImageMt = request.CccdImageFront;
                        }
                        if (!string.IsNullOrWhiteSpace(request.CccdImageBack))
                        {
                            instructor.Cccd.ImageMs = request.CccdImageBack;
                        }
                    }
                }
                
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var instructor = _context.Users.Find(id);
            if (instructor != null)
            {
                _context.Users.Remove(instructor);
                _context.SaveChanges();
            }
        }

        public void AddSpecialization(InstructorSpecialization specialization)
        {
            _context.InstructorSpecializations.Add(specialization);
            _context.SaveChanges();
        }

        public void RemoveSpecialization(int instructorId, byte licenceTypeId)
        {
            var specialization = _context.InstructorSpecializations
                .FirstOrDefault(ins => ins.InstructorId == instructorId && ins.LicenceTypeId == licenceTypeId);
            
            if (specialization != null)
            {
                _context.InstructorSpecializations.Remove(specialization);
                _context.SaveChanges();
            }
        }

        public List<LicenceType> GetAllLicenceTypes()
        {
            return _context.LicenceTypes.ToList();
        }

        public async Task<List<LearnerUserResponse>> GetAllLearnersAsync(string? searchString = null)
        {
            // Get learner role ID
            var learnerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == "learner");
            if (learnerRole == null) return new List<LearnerUserResponse>();

            // Query users with learner role
            var query = _context.Users
                .Include(u => u.UserRoles)
                .Include(u => u.Cccd)
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == learnerRole.RoleId))
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(searchString) ||
                    u.MiddleName.Contains(searchString) ||
                    u.LastName.Contains(searchString) ||
                    u.Email.Contains(searchString) ||
                    u.Phone.Contains(searchString) ||
                    (u.Cccd != null && u.Cccd.CccdNumber.Contains(searchString))
                );
            }

            var users = await query
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            return users.Select(u => new LearnerUserResponse
            {
                UserId = u.UserId,
                FullName = $"{u.LastName} {u.MiddleName} {u.FirstName}".Trim(),
                Email = u.Email ?? "",
                Phone = u.Phone ?? "",
                CccdNumber = u.Cccd?.CccdNumber ?? "",
                Dob = u.Dob,
                Gender = u.Gender,
                CccdImageUrl = u.Cccd != null ? (u.Cccd.ImageMt ?? "") + "|" + (u.Cccd.ImageMs ?? "") : "",
                ProfileImageUrl = u.ProfileImageUrl ?? ""
            }).ToList();
        }

        public async Task<bool> UpdateLearnerScoresAsync(int learningId, int? theory, int? simulation, int? obstacle, int? practical)
        {
            var app = await _context.LearningApplications.FirstOrDefaultAsync(x => x.LearningId == learningId);
            if (app == null) return false;

            var standards = await _context.TestScoreStandards
                .Where(s => s.LicenceTypeId == app.LicenceTypeId)
                .ToListAsync();

            bool isValid = true;

            if (standards.FirstOrDefault(s => s.PartName == "Theory") is { } theoryStd && theory.HasValue)
                isValid &= theory.Value <= theoryStd.MaxScore;

            if (standards.FirstOrDefault(s => s.PartName == "Simulation") is { } simStd && simulation.HasValue)
                isValid &= simulation.Value <= simStd.MaxScore;

            if (standards.FirstOrDefault(s => s.PartName == "Obstacle") is { } obsStd && obstacle.HasValue)
                isValid &= obstacle.Value <= obsStd.MaxScore;

            if (standards.FirstOrDefault(s => s.PartName == "Practical") is { } pracStd && practical.HasValue)
                isValid &= practical.Value <= pracStd.MaxScore;

            if (!isValid) return false;

            app.TheoryScore = theory;
            app.SimulationScore = simulation;
            app.ObstacleScore = obstacle;
            app.PracticalScore = practical;

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<LearningApplicationsResponse>> GetLearningApplicationsByInstructorAsync(int instructorId)
        {
            var learnerIds = await _context.ClassMembers
                .Where(cm => cm.Class.InstructorId == instructorId)
                .Select(cm => cm.LearnerId)
                .Distinct()
                .ToListAsync();

            var applications = await _context.LearningApplications
                .Where(app => learnerIds.Contains(app.LearnerId))
                .Include(app => app.Learner).ThenInclude(l => l.Cccd)
                .Include(app => app.Learner).ThenInclude(l => l.HealthCertificate)
                .Include(app => app.LicenceType)
                .ToListAsync();

            return applications.Select(app => new LearningApplicationsResponse
            {
                LearningId = app.LearningId,
                LearnerId = app.LearnerId,
                LearnerFullName = app.Learner != null ? string.Join(" ", new[] { app.Learner.FirstName, app.Learner.MiddleName, app.Learner.LastName }.Where(n => !string.IsNullOrWhiteSpace(n))) : "",
                LearnerCccdNumber = app.Learner?.Cccd?.CccdNumber,
                LearnerEmail = app.Learner?.Email,
                LicenceTypeId = app.LicenceTypeId,
                LicenceTypeName = app.LicenceType?.LicenceCode,
                SubmittedAt = app.SubmittedAt,
                LearningStatus = app.LearningStatus,
                LearningStatusName = app.LearningStatus switch
                {
                    1 => "Đang học",
                    2 => "Hoàn thành",
                    3 => "Đã huỷ",
                    _ => "Chưa xác định"
                },
                TheoryScore = app.TheoryScore,
                SimulationScore = app.SimulationScore,
                ObstacleScore = app.ObstacleScore,
                PracticalScore = app.PracticalScore
            }).ToList();
        }

    }
} 