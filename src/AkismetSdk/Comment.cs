namespace AkismetSdk
{
    using System;
    using System.Collections.Generic;

    public class Comment
    {
        public Comment(Uri blogUri, string ipAddress, string userAgent)
        {
            BlogUri = blogUri;
            IpAddress = ipAddress;
            UserAgent = userAgent;

            Languages = new List<string>();
        }

        public string Body { get; set; }

        public Uri BlogUri { get; }

        public CommentType CommentType { get; set; }

        public Uri CommentUri { get; set; }

        public DateTime CreatedAt { get; set; }

        public string EmailAddress { get; set; }

        public string Encoding { get; set; }

        public string IpAddress { get; }

        public bool IsTestMode { get; set; }

        public IEnumerable<string> Languages { get; set; }

        public string Name { get; set; }

        public DateTime? PostModifiedAt { get; set; }

        public string Referrer { get; set; }

        public string UserAgent { get; }

        public string UserRole { get; set; }

        public string WebsiteUrl { get; set; }
    }
}