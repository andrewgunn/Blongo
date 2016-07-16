namespace Blongo.Models.ViewPost
{
    public class Commenter
    {
        public Commenter(string name, string emailAddress, string websiteUrl)
        {
            Name = name;
            EmailAddress = emailAddress;
            WebsiteUrl = websiteUrl;
        }

        public string EmailAddress { get; }

        public string Name { get; }

        public string WebsiteUrl { get; }
    }
}
