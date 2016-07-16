namespace Blongo.Areas.Admin.Models.ListComments
{
    public class Commenter
    {
        public Commenter(string name, string emailAddress, string websiteUrl)
        {
            _name = name;
            _emailAddress = emailAddress;
            _websiteUrl = websiteUrl;
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string WebsiteUrl
        {
            get { return _websiteUrl; }
        }

        private readonly string _emailAddress;
        private readonly string _name;
        private readonly string _websiteUrl;
    }
}
