using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.AttendanceRepository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public AttendanceRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAttendanceByClassDateSlotAsync(int classId, DateOnly date, int slotId)
        {
            return await _context.Attendances
                .Include(a => a.Learner)
                    .ThenInclude(la => la.Learner)
                .Include(a => a.Class)
                .Include(a => a.ClassTime)
                .Where(a => a.ClassId == classId && 
                           a.SessionDate == date &&
                           (a.ClassTime == null || a.ClassTime.SlotId == slotId))
                .OrderBy(a => a.Learner.Learner.LastName)
                .ThenBy(a => a.Learner.Learner.FirstName)
                .ToListAsync();
        }

        public async Task<Attendance> CreateAttendanceAsync(Attendance attendance)
        {
            // Find the corresponding ClassTime if not set
            if (attendance.ClassTimeId == null)
            {
                var classTime = await _context.ClassTimes
                    .FirstOrDefaultAsync(ct => ct.ClassId == attendance.ClassId);
                if (classTime != null)
                {
                    attendance.ClassTimeId = classTime.ClassTimeId;
                }
            }

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task CreateAttendanceRangeAsync(List<Attendance> attendanceRecords)
        {
            if (!attendanceRecords.Any()) return;

            // Set ClassTimeId for records that don't have it
            var classIds = attendanceRecords.Select(a => a.ClassId).Distinct().ToList();
            var classTimes = await _context.ClassTimes
                .Where(ct => classIds.Contains(ct.ClassId))
                .ToListAsync();

            foreach (var attendance in attendanceRecords.Where(a => a.ClassTimeId == null))
            {
                var classTime = classTimes.FirstOrDefault(ct => ct.ClassId == attendance.ClassId);
                if (classTime != null)
                {
                    attendance.ClassTimeId = classTime.ClassTimeId;
                }
            }

            _context.Attendances.AddRange(attendanceRecords);
            await _context.SaveChangesAsync();
        }

        public async Task<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            // Check if this entity is already being tracked
            var trackedEntity = _context.ChangeTracker.Entries<Attendance>()
                .FirstOrDefault(e => e.Entity.AttendanceId == attendance.AttendanceId);

            if (trackedEntity != null)
            {
                // Entity is already tracked, update its properties
                var entity = trackedEntity.Entity;
                entity.AttendanceStatus = attendance.AttendanceStatus;
                entity.PracticalDurationHours = attendance.PracticalDurationHours;
                entity.PracticalDistance = attendance.PracticalDistance;
                entity.Note = attendance.Note;
                entity.SessionDate = attendance.SessionDate;
                entity.ClassTimeId = attendance.ClassTimeId;
            }
            else
            {
                // Entity is not tracked, we can safely update it
                _context.Attendances.Update(attendance);
            }

            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task UpdateAttendanceRangeAsync(List<Attendance> attendanceRecords)
        {
            if (!attendanceRecords.Any()) return;

            foreach (var attendance in attendanceRecords)
            {
                // Check if this entity is already being tracked
                var trackedEntity = _context.ChangeTracker.Entries<Attendance>()
                    .FirstOrDefault(e => e.Entity.AttendanceId == attendance.AttendanceId);

                if (trackedEntity != null)
                {
                    // Entity is already tracked, update its properties
                    var entity = trackedEntity.Entity;
                    entity.AttendanceStatus = attendance.AttendanceStatus;
                    entity.PracticalDurationHours = attendance.PracticalDurationHours;
                    entity.PracticalDistance = attendance.PracticalDistance;
                    entity.Note = attendance.Note;
                    entity.SessionDate = attendance.SessionDate;
                    entity.ClassTimeId = attendance.ClassTimeId;
                    
                    // Mark as modified if it's not already
                    if (trackedEntity.State != Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        trackedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }
                else
                {
                    // Entity is not tracked, we can safely update it
                    if (attendance.AttendanceId > 0)
                    {
                        // Existing record - attach and mark as modified
                        _context.Attendances.Attach(attendance);
                        _context.Entry(attendance).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        // New record - add it
                        _context.Attendances.Add(attendance);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAttendanceAsync(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance == null) return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int attendanceId)
        {
            return await _context.Attendances
                .Include(a => a.Learner)
                    .ThenInclude(la => la.Learner)
                .Include(a => a.Class)
                .Include(a => a.ClassTime)
                .FirstOrDefaultAsync(a => a.AttendanceId == attendanceId);
        }

        public async Task<List<Attendance>> GetAttendanceByClassAsync(int classId)
        {
            return await _context.Attendances
                .Include(a => a.Learner)
                    .ThenInclude(la => la.Learner)
                .Include(a => a.ClassTime)
                .Where(a => a.ClassId == classId)
                .OrderBy(a => a.SessionDate)
                .ThenBy(a => a.Learner.Learner.LastName)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceByLearnerAsync(int learnerId)
        {
            return await _context.Attendances
                .Include(a => a.Class)
                .Include(a => a.ClassTime)
                .Where(a => a.LearnerId == learnerId)
                .OrderBy(a => a.SessionDate)
                .ToListAsync();
        }

        public async Task<List<Attendance>> GetAttendanceByDateRangeAsync(int classId, DateOnly startDate, DateOnly endDate)
        {
            return await _context.Attendances
                .Include(a => a.Learner)
                    .ThenInclude(la => la.Learner)
                .Include(a => a.ClassTime)
                .Where(a => a.ClassId == classId &&
                           a.SessionDate >= startDate &&
                           a.SessionDate <= endDate)
                .OrderBy(a => a.SessionDate)
                .ThenBy(a => a.Learner.Learner.LastName)
                .ToListAsync();
        }

        public async Task<bool> AttendanceExistsAsync(int classId, int learnerId, DateOnly date, int slotId)
        {
            return await _context.Attendances
                .Include(a => a.ClassTime)
                .AnyAsync(a => a.ClassId == classId &&
                              a.LearnerId == learnerId &&
                              a.SessionDate == date &&
                              (a.ClassTime == null || a.ClassTime.SlotId == slotId));
        }

        public async Task<bool> AttendanceExistsForClassTimeAsync(int classId, DateOnly date, int classTimeId)
        {
            return await _context.Attendances
                .AsNoTracking()
                .AnyAsync(a => a.ClassId == classId &&
                              a.SessionDate == date &&
                              a.ClassTimeId == classTimeId);
        }
    }
}