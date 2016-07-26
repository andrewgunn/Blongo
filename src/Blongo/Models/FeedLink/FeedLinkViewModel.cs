namespace Blongo.Models.FeedLink
{
    public class FeedLinkViewModel
    {
        public FeedLinkViewModel(Blog blog)
        {
            Blog = blog;
        }

        public Blog Blog { get; }
    }
}
