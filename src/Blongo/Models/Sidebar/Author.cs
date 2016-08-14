using System;

namespace Blongo.Models.Sidebar
{
    public class Author
    {
        public Author(string name, Uri websiteUrl, string emailAddress, string gitHubUsername, string twitterUsername)
        {
            Name = name;
            WebsiteUrl = websiteUrl;
            EmailAddress = emailAddress;
            GitHubUsername = gitHubUsername;
            TwitterUsername = twitterUsername;
        }

        public string EmailAddress { get; }

        public string GitHubUsername{ get; }

        public string Name { get; }

        public string TwitterUsername { get; }

        public Uri WebsiteUrl { get; }
    }
}
