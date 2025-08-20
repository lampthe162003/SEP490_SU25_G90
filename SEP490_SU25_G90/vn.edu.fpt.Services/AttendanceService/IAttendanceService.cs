using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.AttendanceService
{
    /// <summary>
    /// Interface for Attendance business logic operations
    /// </summary>
    public interface IAttendanceService
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
        /// Get attendance records for a specific class time and date
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="date">Session date</param>
        /// <param name="classTimeId">ClassTime ID for specific schedule</param>
        /// <returns>List of attendance records</returns>
        Task<List<Attendance>> GetAttendanceByClassTimeAsync(int classId, DateOnly date, int classTimeId);

        /// <summary>
        /// Create new attendance records
        /// </summary>
        /// <param name="attendanceRecords">List of attendance records to create</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task CreateAttendanceAsync(List<Attendance> attendanceRecords);

        /// <summary>
        /// Update existing attendance records
        /// </summary>
        /// <param name="attendanceRecords">List of attendance records to update</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task UpdateAttendanceAsync(List<Attendance> attendanceRecords);

        /// <summary>
        /// Delete attendance record by ID
        /// </summary>
        /// <param name="attendanceId">Attendance ID to delete</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteAttendanceAsync(int attendanceId);

        /// <summary>
        /// Get attendance record by ID
        /// </summary>
        /// <param name="attendanceId">Attendance ID</param>
        /// <returns>Attendance record or null</returns>
        Task<Attendance?> GetAttendanceByIdAsync(int attendanceId);

        /// <summary>
        /// Get attendance statistics for a class
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="startDate">Start date range</param>
        /// <param name="endDate">End date range</param>
        /// <returns>Attendance statistics</returns>
        Task<(int TotalSessions, int TotalPresent, int TotalAbsent, double AttendanceRate)> GetAttendanceStatisticsAsync(
            int classId, DateOnly? startDate = null, DateOnly? endDate = null);

        /// <summary>
        /// Check if attendance exists for a specific class time and date
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <param name="date">Session date</param>
        /// <param name="classTimeId">ClassTime ID</param>
        /// <returns>True if attendance exists</returns>
        Task<bool> AttendanceExistsForClassTimeAsync(int classId, DateOnly date, int classTimeId);
    }
}