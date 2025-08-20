using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.AttendanceRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.AttendanceService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public async Task<List<Attendance>> GetAttendanceByClassDateSlotAsync(int classId, DateOnly date, int slotId)
        {
            return await _attendanceRepository.GetAttendanceByClassDateSlotAsync(classId, date, slotId);
        }

        public async Task<List<Attendance>> GetAttendanceByClassTimeAsync(int classId, DateOnly date, int classTimeId)
        {
            // Use the repository method that filters by ClassTimeId specifically
            var allAttendance = await _attendanceRepository.GetAttendanceByClassAsync(classId);
            return allAttendance.Where(a => a.SessionDate == date && a.ClassTimeId == classTimeId).ToList();
        }

        public async Task CreateAttendanceAsync(List<Attendance> attendanceRecords)
        {
            if (attendanceRecords == null || !attendanceRecords.Any())
                throw new ArgumentException("Attendance records cannot be null or empty");

            // Validate attendance records
            foreach (var attendance in attendanceRecords)
            {
                if (attendance.ClassId <= 0)
                    throw new ArgumentException("Invalid ClassId in attendance record");
                
                if (attendance.LearnerId <= 0)
                    throw new ArgumentException("Invalid LearnerId in attendance record");
                
                // Check for duplicate entries
                var exists = await _attendanceRepository.AttendanceExistsAsync(
                    attendance.ClassId, 
                    attendance.LearnerId, 
                    attendance.SessionDate, 
                    0); // We'll handle slot validation in repository
                
                if (exists)
                    throw new InvalidOperationException($"Attendance already exists for learner {attendance.LearnerId} on {attendance.SessionDate}");
            }

            await _attendanceRepository.CreateAttendanceRangeAsync(attendanceRecords);
        }

        public async Task UpdateAttendanceAsync(List<Attendance> attendanceRecords)
        {
            if (attendanceRecords == null || !attendanceRecords.Any())
                throw new ArgumentException("Attendance records cannot be null or empty");

            var recordsToUpdate = new List<Attendance>();
            var recordsToCreate = new List<Attendance>();

            foreach (var attendance in attendanceRecords)
            {
                if (attendance.ClassId <= 0)
                    throw new ArgumentException("Invalid ClassId in attendance record");
                
                if (attendance.LearnerId <= 0)
                    throw new ArgumentException("Invalid LearnerId in attendance record");

                if (attendance.AttendanceId > 0)
                {
                    // Update existing record
                    recordsToUpdate.Add(attendance);
                }
                else
                {
                    // Create new record
                    recordsToCreate.Add(attendance);
                }
            }

            // Update existing records
            if (recordsToUpdate.Any())
            {
                await _attendanceRepository.UpdateAttendanceRangeAsync(recordsToUpdate);
            }

            // Create new records
            if (recordsToCreate.Any())
            {
                await _attendanceRepository.CreateAttendanceRangeAsync(recordsToCreate);
            }
        }

        public async Task DeleteAttendanceAsync(int attendanceId)
        {
            if (attendanceId <= 0)
                throw new ArgumentException("Invalid attendance ID");

            var success = await _attendanceRepository.DeleteAttendanceAsync(attendanceId);
            if (!success)
                throw new InvalidOperationException("Attendance record not found or could not be deleted");
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(int attendanceId)
        {
            if (attendanceId <= 0)
                throw new ArgumentException("Invalid attendance ID");

            return await _attendanceRepository.GetAttendanceByIdAsync(attendanceId);
        }

        public async Task<(int TotalSessions, int TotalPresent, int TotalAbsent, double AttendanceRate)> GetAttendanceStatisticsAsync(
            int classId, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            if (classId <= 0)
                throw new ArgumentException("Invalid class ID");

            List<Attendance> attendanceRecords;

            if (startDate.HasValue && endDate.HasValue)
            {
                attendanceRecords = await _attendanceRepository.GetAttendanceByDateRangeAsync(classId, startDate.Value, endDate.Value);
            }
            else
            {
                attendanceRecords = await _attendanceRepository.GetAttendanceByClassAsync(classId);
            }

            var totalSessions = attendanceRecords.Count;
            var totalPresent = attendanceRecords.Count(a => a.AttendanceStatus == true);
            var totalAbsent = attendanceRecords.Count(a => a.AttendanceStatus == false);
            var attendanceRate = totalSessions > 0 ? (double)totalPresent / totalSessions * 100 : 0;

            return (totalSessions, totalPresent, totalAbsent, attendanceRate);
        }

        public async Task<bool> AttendanceExistsForClassTimeAsync(int classId, DateOnly date, int classTimeId)
        {
            if (classId <= 0)
                throw new ArgumentException("Invalid class ID");

            return await _attendanceRepository.AttendanceExistsForClassTimeAsync(classId, date, classTimeId);
        }
    }
}