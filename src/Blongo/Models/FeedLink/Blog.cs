namespace Blongo.Models.FeedLink
{
    public class Blog
    {
        public Blog(string name, string feedUrl)
        {
            Name = name;
            FeedUrl = feedUrl;
        }

        public string FeedUrl { get; }

        public string Name { get; }
    }
}
