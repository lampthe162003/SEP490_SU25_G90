using System.ComponentModel.DataAnnotations;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class UpdateLearnerProgressRequest
    {
        public int LearningId { get; set; }

        [Range(float.Epsilon, 24)]
        public float PracticalDurationHours { get; set; }

        [Range(float.Epsilon, float.PositiveInfinity)]
        public float PracticalDistance {  get; set; }
    }
}
