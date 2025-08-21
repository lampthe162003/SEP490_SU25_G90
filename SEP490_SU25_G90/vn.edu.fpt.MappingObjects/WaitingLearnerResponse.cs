namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class WaitingLearnerResponse
    {
        public int LearningId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Cccd { get; set; } = string.Empty;
        public string Status { get; set; } = "Chưa học";
        public string ProfileImageUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/1144/1144760.png";
        public bool Selected { get; set; }
        public byte? LicenceTypeId { get; set; }
        public byte? LearningStatus { get; set; }
    }
}
