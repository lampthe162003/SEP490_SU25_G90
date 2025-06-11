using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class CourseInformationResponse
    {
        public string? Title { get; set; }

        public string? Description { get; set; }


        public bool? ActiveStatus { get; set; }
        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();

    }
}
