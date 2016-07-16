using FluentAssertions;
using System.Collections.Specialized;

namespace AkismetSdk.Tests
{
    public static class FormDataExtensions
    {
        public static void ShouldBeEquivalentTo(this NameValueCollection formData, Comment comment)
        {
            formData["blog"].Should().Be(comment.BlogUri.AbsoluteUri);
            formData["comment_type"].Should().Be(new CommentTypeToString().Map(comment.CommentType));
            formData["comment_author"].Should().Be(comment.Name);
            formData["comment_author_email"].Should().Be(comment.EmailAddress);
            formData["comment_author_url"].Should().Be(comment.WebsiteUrl);
            formData["comment_content"].Should().Be(comment.Body);
            formData["permalink"].Should().Be(comment.CommentUri.AbsoluteUri);
            formData["referrer"].Should().Be(comment.Referrer);
            formData["user_ip"].Should().Be(comment.IpAddress);
            formData["user_agent"].Should().Be(comment.UserAgent);
            formData["comment_date_gmt"].Should().Be(comment.CreatedAt.ToString("s"));

            if (comment.PostModifiedAt == null)
            {
                formData["comment_post_modified_gmt"].Should().BeNull();
            }
            else
            {
                formData["comment_post_modified_gmt"].Should().Be(comment.PostModifiedAt.Value.ToString("s"));
            }

            formData["blog_lang"].Should().Be(string.Join(",", comment.Languages));
            formData["blog_charset"].Should().Be(comment.Encoding);
            formData["user_role"].Should().Be(comment.UserRole);
            formData["is_test"].Should().Be(comment.IsTestMode.ToString().ToLowerInvariant());
        }
    }
}
