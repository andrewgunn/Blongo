using System;

namespace Blongo
{
    public class SitemapUrl
    {
        public SitemapChangeFrequency? ChangeFrequency { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        public double? Priority { get; set; }

        public string Url { get; set; }
    }
}
