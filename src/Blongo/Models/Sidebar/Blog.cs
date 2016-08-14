namespace Blongo.Models.Sidebar
{
    public class Blog
    {
        public Blog(string name, string description, Company company, string feedUrl, Author author)
        {
            Name = name;
            Description = description;
            Company = company;
            FeedUrl = feedUrl;
            Author = author;
        }

        public Author Author { get; }

        public Company Company { get; }

        public string Description { get; }

        public string FeedUrl { get; }

        public string Name { get; }
    }
}
