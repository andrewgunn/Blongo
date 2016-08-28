namespace Blongo.Areas.Admin.Models.ListComments
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