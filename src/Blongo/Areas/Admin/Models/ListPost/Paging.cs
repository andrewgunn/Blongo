namespace Blongo.Areas.Admin.Models.ListPosts
{
    public class Paging
    {
        public Paging(int pageNumber, int pageSize, int maximumPageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            MaximumPageNumber = maximumPageNumber;
        }

        public int MaximumPageNumber { get; }

        public int PageNumber { get; }

        public int PageSize { get; }
    }
}
