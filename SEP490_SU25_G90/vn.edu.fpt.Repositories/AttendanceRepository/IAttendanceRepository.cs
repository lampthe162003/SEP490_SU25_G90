using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.AttendanceRepository
{
    /// <summary>
    /// Interface for Attendance data access operations
    /// </summary>
    public interface IAttendanceRepository
    {
        /// <summary>
        /// Get attendance records for a specific class, date, and slot
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="date">Session date</param>
        /// <param name="slotId">Slot ID</param>
        /// <returns>List of attendance records</returns>
        Task<List<Attendance>> GetAttendanceByClassDateSlotAsync(int classId, DateOnly date, int slotId);

        /// <summary>
        /// Create new attendance record
        /// </summary>
        /// <param name="attendance">Attendance record to create</param>
        /// <returns>Created attendance record</returns>
        Task<Attendance> CreateAttendanceAsync(Attendance attendance);

        /// <summary>
        /// Create multiple attendance records
        /// </summary>
        /// <param name="attendanceRecords">List of attendance records to create</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task CreateAttendanceRangeAsync(List<Attendance> attendanceRecords);

        /// <summary>
        /// Update existing attendance record
        /// </summary>
        /// <param name="attendance">Attendance record to update</param>
        /// <returns>Updated attendance record</returns>
        Task<Attendance> UpdateAttendanceAsync(Attendance attendance);

        /// <summary>
        /// Update multiple attendance records
        /// </summary>
        /// <param name="attendanceRecords">List of attendance records to update</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateAttendanceRangeAsync(List<Attendance> attendanceRecords);

        /// <summary>
        /// Delete attendance record by ID
        /// </summary>
        /// <param name="attendanceId">Attendance ID to delete</param>
        /// <returns>True if deleted successfully</returns>
        Task<bool> DeleteAttendanceAsync(int attendanceId);

        /// <summary>
        /// Get attendance record by ID
        /// </summary>
        /// <param name="attendanceId">Attendance ID</param>
        /// <returns>Attendance record or null</returns>
        Task<Attendance?> GetAttendanceByIdAsync(int attendanceId);

        /// <summary>
        /// Get all attendance records for a class
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <returns>List of attendance records</returns>
        Task<List<Attendance>> GetAttendanceByClassAsync(int classId);

        /// <summary>
        /// Get attendance records for a learner
        /// </summary>
        /// <param name="learnerId">Learner ID</param>
        /// <returns>List of attendance records</returns>
        Task<List<Attendance>> GetAttendanceByLearnerAsync(int learnerId);

        /// <summary>
        /// Get attendance records for a date range
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>List of attendance records</returns>
        Task<List<Attendance>> GetAttendanceByDateRangeAsync(int classId, DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Check if attendance exists for a specific class, learner, and date
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="learnerId">Learner ID</param>
        /// <param name="date">Session date</param>
        /// <param name="slotId">Slot ID</param>
        /// <returns>True if attendance exists</returns>
        Task<bool> AttendanceExistsAsync(int classId, int learnerId, DateOnly date, int slotId);

        /// <summary>
        /// Check if attendance exists for a specific class time and date (no tracking)
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="date">Session date</param>
        /// <param name="classTimeId">ClassTime ID</param>
        /// <returns>True if attendance exists</returns>
        Task<bool> AttendanceExistsForClassTimeAsync(int classId, DateOnly date, int classTimeId);
    }
}