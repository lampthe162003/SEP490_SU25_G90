using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class TestApplicationListInformationResponse
    {
        public int TestId { get; set; }
        public DateOnly? ExamDate { get; set; }
        public bool? Status { get; set; }
        public string LearnerFullName => Learning == null || Learning.Learner == null
           ? string.Empty
           : $"{Learning.Learner.FirstName} {Learning.Learner.MiddleName} {Learning.Learner.LastName}";

        public string CccdNumber => Learning?.Learner?.Cccd?.CccdNumber ?? string.Empty;

        public string LicenceType => Learning?.LicenceType?.LicenceCode ?? string.Empty;
        public virtual LearningApplication? Learning { get; set; }
    }
}
