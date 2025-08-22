using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.AttendanceService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using SEP490_SU25_G90.vn.edu.fpt.Services.ScheduleSlotService;

namespace SEP490_SU25_G90.Pages.Instructors.RollCall
{
    [Authorize(Roles = "instructor")]
    public class AttendanceModel : PageModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IClassService _classService;
        private readonly IScheduleSlotService _scheduleSlotService;
        private readonly ILearningApplicationService _learningApplicationService;

        public AttendanceModel(
            IAttendanceService attendanceService,
            IClassService classService,
            IScheduleSlotService scheduleSlotService,
            ILearningApplicationService learningApplicationService)
        {
            _attendanceService = attendanceService;
            _classService = classService;
            _scheduleSlotService = scheduleSlotService;
            _learningApplicationService = learningApplicationService;
        }

        [BindProperty(SupportsGet = true)]
        public int ClassId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly Date { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SlotId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ClassTimeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Year { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? StartOfWeekInput { get; set; }

        [BindProperty]
        public List<StudentAttendanceForm> Students { get; set; } = new();

        public string ClassName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public string SlotTime { get; set; } = string.Empty;
        public bool IsEditMode { get; set; }

        public async Task<IActionResult> OnGetAsync(int classId, string date, int slotId, int classTimeId, int? year = null, string? startOfWeekInput = null)
        {
            // Parse parameters
            ClassId = classId;
            SlotId = slotId;
            ClassTimeId = classTimeId;
            Year = year;
            StartOfWeekInput = startOfWeekInput;

            if (!DateOnly.TryParse(date, out var parsedDate))
            {
                TempData["ErrorMessage"] = "Ngày không hợp lệ.";
                return Redirect(GetReturnUrl());
            }
            Date = parsedDate;

            // Verify instructor has permission for this class
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out int instructorId) || instructorId == 0)
            {
                return Unauthorized();
            }

            // Get class details
            var classDetails = await _classService.GetClassByIdAsync(ClassId);
            if (classDetails == null || classDetails.InstructorId != instructorId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy lớp học hoặc bạn không có quyền truy cập.";
                return Redirect(GetReturnUrl());
            }

            ClassName = classDetails.ClassName ?? "Unknown Class";
            CourseName = classDetails.Course?.CourseName ?? "Unknown Course";

            // Get slot details
            var slot = await _scheduleSlotService.GetSlotById(SlotId);
            if (slot != null)
            {
                SlotTime = $"{slot.StartTime?.ToString(@"HH\:mm")} - {slot.EndTime?.ToString(@"HH\:mm")}";
            }

            // Get class members
            var classMembers = await _classService.GetClassMembersAsync(ClassId);

            // Check if attendance exists for this class time without loading entities (to avoid tracking issues)
            IsEditMode = await _attendanceService.AttendanceExistsForClassTimeAsync(ClassId, Date, ClassTimeId);

            // Get existing attendance records only if we're in edit mode
            List<Attendance> existingAttendance = new();
            if (IsEditMode)
            {
                existingAttendance = await _attendanceService.GetAttendanceByClassTimeAsync(ClassId, Date, ClassTimeId);
            }

            // Build student list with attendance status
            Students = classMembers.Select(member =>
            {
                var existingRecord = existingAttendance.FirstOrDefault(a => a.LearnerId == member.LearnerId);
                return new StudentAttendanceForm
                {
                    LearnerId = member.LearnerId ?? 0,
                    StudentName = GetStudentName(member.Learner),
                    AttendanceStatus = existingRecord?.AttendanceStatus,
                    PracticalDurationHours = existingRecord?.PracticalDurationHours,
                    PracticalDistance = existingRecord?.PracticalDistance,
                    Note = existingRecord?.Note ?? string.Empty,
                    AttendanceId = existingRecord?.AttendanceId
                };
            }).OrderBy(s => s.StudentName).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAttendanceAsync()
        {
            // Verify instructor has permission for this class
            var userIdClaim = User.FindFirst("user_id")?.Value;
            if (!int.TryParse(userIdClaim, out int instructorId) || instructorId == 0)
            {
                return Unauthorized();
            }

            var classDetails = await _classService.GetClassByIdAsync(ClassId);
            if (classDetails == null || classDetails.InstructorId != instructorId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy lớp học hoặc bạn không có quyền truy cập.";
                return Redirect(GetReturnUrl());
            }

            // Custom validation - only validate practical data when student is marked as present
            var validationErrors = new List<string>();
            
            for (int i = 0; i < Students.Count; i++)
            {
                var student = Students[i];
                
                // Only validate practical hours and distance if student is marked as present
                if (student.AttendanceStatus == true)
                {
                    // Validate practical hours for present students
                    if (!student.PracticalDurationHours.HasValue || student.PracticalDurationHours <= 0)
                    {
                        validationErrors.Add($"Học viên {student.StudentName}: Vui lòng nhập số giờ thực hành khi có mặt.");
                    }
                    else if (student.PracticalDurationHours > 24)
                    {
                        validationErrors.Add($"Học viên {student.StudentName}: Số giờ thực hành không được vượt quá 24 giờ.");
                    }
                    
                    // Validate practical distance for present students
                    if (!student.PracticalDistance.HasValue || student.PracticalDistance <= 0)
                    {
                        validationErrors.Add($"Học viên {student.StudentName}: Vui lòng nhập quãng đường thực hành khi có mặt.");
                    }
                }
                
                // Clear practical data for absent students
                if (student.AttendanceStatus == false)
                {
                    student.PracticalDurationHours = 0;
                    student.PracticalDistance = 0;
                }
            }
            
            if (validationErrors.Any())
            {
                TempData["ErrorMessage"] = string.Join("<br/>", validationErrors);
                await OnGetAsync(ClassId, Date.ToString("yyyy-MM-dd"), SlotId, ClassTimeId, Year, StartOfWeekInput);
                return Page();
            }

            try
            {
                // Prepare attendance records - include all students (even those without attendance marked)
                var attendanceRecords = Students.Select(s => new Attendance
                {
                    AttendanceId = s.AttendanceId ?? 0,
                    LearnerId = s.LearnerId,
                    ClassId = ClassId,
                    SessionDate = Date,
                    ClassTimeId = ClassTimeId, // Use the specific ClassTimeId passed from schedule
                    AttendanceStatus = s.AttendanceStatus, // Can be null, true, or false
                    PracticalDurationHours = s.PracticalDurationHours,
                    PracticalDistance = s.PracticalDistance,
                    Note = string.IsNullOrEmpty(s.Note) ? null : s.Note
                }).ToList();

                // Double-check if this is a new or updated roll call
                bool hasExistingRecords = await _attendanceService.AttendanceExistsForClassTimeAsync(ClassId, Date, ClassTimeId);
                
                // Count students being marked for success message
                var studentsBeingMarked = attendanceRecords.Count(r => r.AttendanceStatus.HasValue);
                
                if (hasExistingRecords)
                {
                    // This is an update operation
                    await _attendanceService.UpdateAttendanceAsync(attendanceRecords);
                    var presentCount = attendanceRecords.Count(r => r.AttendanceStatus == true);
                    var absentCount = attendanceRecords.Count(r => r.AttendanceStatus == false);
                    TempData["SuccessMessage"] = $"Cập nhật điểm danh thành công! Đã cập nhật {studentsBeingMarked} học viên " +
                        $"({presentCount} có mặt, {absentCount} vắng) cho buổi học ngày {Date:dd/MM/yyyy}.";
                    
                    //Update learner's total progress into learning application
                    foreach (var attendanceRecord in attendanceRecords)
                    {
                        UpdateLearnerProgressRequest request = new UpdateLearnerProgressRequest
                        {
                            LearningId = attendanceRecord.LearnerId,
                            PracticalDistance = (float)attendanceRecord.PracticalDistance,
                            PracticalDurationHours = (float)attendanceRecord?.PracticalDurationHours
                        };
                        await _learningApplicationService.UpdateLearnerProgress(request);
                    }
                }
                else
                {
                    // This is a new roll call creation
                    var recordsToCreate = attendanceRecords.Where(r => r.AttendanceStatus.HasValue).ToList();
                    
                    if (recordsToCreate.Any())
                    {
                        await _attendanceService.CreateAttendanceAsync(recordsToCreate);
                        var presentCount = recordsToCreate.Count(r => r.AttendanceStatus == true);
                        var absentCount = recordsToCreate.Count(r => r.AttendanceStatus == false);
                        TempData["SuccessMessage"] = $"Tạo điểm danh mới thành công! Đã điểm danh {recordsToCreate.Count} học viên " +
                            $"({presentCount} có mặt, {absentCount} vắng) cho buổi học ngày {Date:dd/MM/yyyy}.";
                        foreach (var attendanceRecord in attendanceRecords)
                        {
                            UpdateLearnerProgressRequest request = new UpdateLearnerProgressRequest
                            {
                                LearningId = attendanceRecord.LearnerId,
                                PracticalDistance = (float)attendanceRecord.PracticalDistance,
                                PracticalDurationHours = (float)attendanceRecord?.PracticalDurationHours
                            };
                            await _learningApplicationService.UpdateLearnerProgress(request);
                        }
                    }
                    else
                    {
                        TempData["WarningMessage"] = "Không có dữ liệu điểm danh nào được lưu. Vui lòng chọn trạng thái điểm danh cho ít nhất một học viên trước khi lưu.";
                    }
                }

                return Redirect(GetReturnUrl());
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi lưu điểm danh: {ex.Message}";
                
                // Log the full exception for debugging
                Console.WriteLine($"Attendance Save Error: {ex}");
                
                await OnGetAsync(ClassId, Date.ToString("yyyy-MM-dd"), SlotId, ClassTimeId, Year, StartOfWeekInput);
                return Page();
            }
        }

        private string GetStudentName(LearningApplication learner)
        {
            if (learner?.Learner == null) return "Unknown Student";

            var parts = new[]
            {
                learner.Learner.FirstName,
                learner.Learner.MiddleName,
                learner.Learner.LastName
            }.Where(part => !string.IsNullOrWhiteSpace(part));

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Build the return URL to the instructor schedule with preserved parameters
        /// </summary>
        /// <returns>URL string for instructor schedule</returns>
        public string GetReturnUrl()
        {
            var baseUrl = "/Instructor/Schedule";
            
            if (Year.HasValue || !string.IsNullOrEmpty(StartOfWeekInput))
            {
                var queryParams = new List<string>();
                
                if (Year.HasValue)
                {
                    queryParams.Add($"Year={Year.Value}");
                }
                
                if (!string.IsNullOrEmpty(StartOfWeekInput))
                {
                    queryParams.Add($"StartOfWeekInput={StartOfWeekInput}");
                }
                
                if (queryParams.Any())
                {
                    baseUrl += "?" + string.Join("&", queryParams);
                }
            }
            
            return baseUrl;
        }
    }

    public class StudentAttendanceForm
    {
        public int LearnerId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public bool? AttendanceStatus { get; set; }
        public double? PracticalDurationHours { get; set; }
        public double? PracticalDistance { get; set; }
        public string Note { get; set; } = string.Empty;
        public int? AttendanceId { get; set; }
    }
}