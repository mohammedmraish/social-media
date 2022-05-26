namespace soicalMedia.Helpers
{
    public class LikesParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _PageSize = 10;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        //------------------------------
        public int UserId { get; set; } 
        public string Predicate { get; set; }
    }
}
