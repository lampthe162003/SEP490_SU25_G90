using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class TestApplicationListInformationResponse
    {
        public int TestId { get; set; }
        public DateOnly? ExamDate { get; set; }
        public double? Score { get; set; }
        public bool? Status { get; set; }
        public string LearnerFullName => Learner == null || Learner.Learner == null
           ? string.Empty
           : $"{Learner.Learner.FirstName} {Learner.Learner.MiddleName} {Learner.Learner.LastName}";

        public string CccdNumber => Learner?.Learner?.Cccd?.CccdNumber ?? string.Empty;

        public string LicenceType => Learner?.LicenceType?.LicenceCode ?? string.Empty;
        public virtual LearningApplication? Learner { get; set; }
    }
}
