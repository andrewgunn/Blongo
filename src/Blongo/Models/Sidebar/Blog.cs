namespace Blongo.Models.Sidebar
{
    public class Blog
    {
        public Blog(string name, string description, string feedUrl, Author author)
        {
            Name = name;
            Author = author;
        }

        public Author Author { get; }

        public string Description { get; }

        public string FeedUrl { get; }

        public string Name { get; }
    }
}
