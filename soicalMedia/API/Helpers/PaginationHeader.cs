namespace soicalMedia.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currnetPage, int itemsPerPage, int totalItems, int totalPages)
        {
            CurrnetPage = currnetPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPages;
        }
        public int CurrnetPage { get; set; }
        public int ItemsPerPage { get; set; }

        public int TotalItems { get; set; }
        public int TotalPages { get; set; }


    }
}
