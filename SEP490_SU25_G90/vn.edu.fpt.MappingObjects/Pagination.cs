namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects
{
    public class Pagination<T>
    {
        public Pagination()
        {
            Data = Enumerable.Empty<T>();
        }
        public IEnumerable<T> Data { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }
    }
}
