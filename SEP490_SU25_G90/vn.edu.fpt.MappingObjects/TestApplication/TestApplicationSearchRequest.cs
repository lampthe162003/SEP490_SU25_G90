namespace SEP490_SU25_G90.vn.edu.fpt.MappingObjects.TestApplication
{
    public class TestApplicationSearchRequest
    {
        public TestApplicationSearchRequest()
        {
            _page = 1;
        }
        public string? Search { get; set; }
        private int _page;
        public int? Page
        {
            get => _page;
            set
            {
                if (!value.HasValue || value.Value < 1)
                {
                    _page = 1;
                }
                _page = value.Value;
            }
        }
        public int? LicenseTypeId { get; set; }
        public int? Status { get; set; }
        public const int Take = 10;
        public int Skip => Take * ((Page ?? 1) - 1);

    }
}
