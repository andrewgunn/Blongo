namespace AkismetSdk.Tests.Clients
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using AkismetSdk.Clients.CommentCheck;
    using FluentAssertions;
    using NUnit.Framework;

    public class CommentCheckClientTests
    {
        [Test]
        public void CommentCheckClient_Post()
        {
            var akismetApiKey = Guid.NewGuid().ToString();

            Uri requestUri = null;
            NameValueCollection requestFormData = null;

            var httpClient = HttpClientFactory.Create(new TestHandler(r =>
            {
                requestUri = r.RequestUri;
                requestFormData = r.Content.ReadAsFormDataAsync().GetAwaiter().GetResult();

                return new HttpResponseMessage
                {
                    Content = new StringContent("true"),
                    StatusCode = HttpStatusCode.OK
                };
            }));

            var akismetSettings = new AkismetSettings(akismetApiKey);
            var client = new CommentCheckClient(akismetSettings, httpClient);

            var blogUri = "http://blog.andrewgunn.co.uk/";
            var commentType = CommentType.Comment;
            var name = "Andrew Gunn";
            var emailAddress = "hello@andrewgunn.co.uk";
            var websiteUrl = "http://andrewgunn.co.uk/";
            var body = "This is a test.";
            var commentUri = "http://blog.andrewgunn.co.uk/post/123/test";
            var referrer = "https://www.google.co.uk/";
            var ipAddress = "127.0.0.1";
            var userAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            var createdAt = "9999-12-31T23:59:59";
            var postModifiedAt = "9999-12-31T23:59:59";
            var userRole = "user";
            var languages = "en,en-GB";
            var encoding = "UTF-8";
            var isTestMode = "true";
            var comment = new Comment(new Uri(blogUri), ipAddress, userAgent);
            comment.CommentType = commentType;
            comment.Name = name;
            comment.EmailAddress = emailAddress;
            comment.WebsiteUrl = websiteUrl;
            comment.Body = body;
            comment.CommentUri = new Uri(commentUri);
            comment.Referrer = referrer;
            comment.CreatedAt = DateTime.Parse(createdAt);
            comment.PostModifiedAt = DateTime.Parse(postModifiedAt);
            comment.Languages = languages.Split(',');
            comment.Encoding = encoding;
            comment.UserRole = userRole;
            comment.IsTestMode = bool.Parse(isTestMode);

            var result =
                client.PostAsync(comment, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

            result.IsSpam.Should().BeTrue();

            requestUri.AbsoluteUri.Should().Be($"https://{akismetApiKey}.rest.akismet.com/1.1/comment-check");

            requestFormData.ShouldBeEquivalentTo(comment);
        }
    }
}