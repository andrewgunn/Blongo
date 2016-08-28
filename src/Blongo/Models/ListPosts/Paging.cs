namespace Blongo.Models.ListPosts
{
    public class Paging
    {
        public Paging(int? previousPageNumber, int? nextPageNumber)
        {
            PreviousPageNumber = previousPageNumber;
            NextPageNumber = nextPageNumber;
        }

        public int? NextPageNumber { get; }

        public int? PreviousPageNumber { get; }
    }
}