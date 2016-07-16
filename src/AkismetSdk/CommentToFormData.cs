using System.Collections.Generic;

namespace AkismetSdk
{
    public class CommentToFormData
    {
        public IEnumerable<KeyValuePair<string, string>> Map(Comment comment)
        {
            return new[]
            {
                new KeyValuePair<string, string>("blog", comment.BlogUri.AbsoluteUri),
                new KeyValuePair<string, string>("comment_type", new CommentTypeToString().Map(comment.CommentType)),
                new KeyValuePair<string, string>("comment_author", comment.Name),
                new KeyValuePair<string, string>("comment_author_email", comment.EmailAddress),
                new KeyValuePair<string, string>("comment_author_url", comment.WebsiteUrl),
                new KeyValuePair<string, string>("comment_content", comment.Body),
                new KeyValuePair<string, string>("permalink", comment.CommentUri?.AbsoluteUri),
                new KeyValuePair<string, string>("referrer", comment.Referrer),
                new KeyValuePair<string, string>("user_ip", comment.IpAddress),
                new KeyValuePair<string, string>("user_agent", comment.UserAgent),
                new KeyValuePair<string, string>("comment_date_gmt", comment.CreatedAt.ToString("s")),
                new KeyValuePair<string, string>("comment_post_modified_gmt", comment.PostModifiedAt == null ? null : comment.PostModifiedAt.Value.ToString("s")),
                new KeyValuePair<string, string>("user_role", comment.UserRole),
                new KeyValuePair<string, string>("blog_lang", string.Join(",", comment.Languages)),
                new KeyValuePair<string, string>("blog_charset", comment.Encoding),
                new KeyValuePair<string, string>("is_test", comment.IsTestMode.ToString().ToLowerInvariant())
            };
        }
    }
}
