using System;

namespace Blongo.Models.Sidebar
{
    public class Company
    {
        public Company(string name, Uri websiteUrl)
        {
            Name = name;
            WebsiteUrl = websiteUrl;
        }

        public string Name { get; }

        public Uri WebsiteUrl { get; }
    }
}
