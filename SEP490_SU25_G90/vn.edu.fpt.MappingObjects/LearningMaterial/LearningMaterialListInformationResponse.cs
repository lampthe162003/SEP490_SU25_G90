namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial
{
    public class LearningMaterialListInformationResponse
    {
        public int MaterialId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? LicenceTypeName { get; set; }
        public string? FileLink { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
