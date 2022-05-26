namespace soicalMedia.Helpers
{
    public class MassageParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _PageSize = 10;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        //-----------------------------
        public string? Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
