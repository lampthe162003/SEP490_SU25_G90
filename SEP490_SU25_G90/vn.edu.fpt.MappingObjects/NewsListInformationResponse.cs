namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class NewsListInformationResponse
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty ;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime? PostTime { get; set; }
        public string ShortContent { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
