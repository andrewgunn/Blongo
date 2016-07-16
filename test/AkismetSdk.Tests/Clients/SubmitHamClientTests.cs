using AkismetSdk.Clients.SubmitHam;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace AkismetSdk.Tests.Clients
{
    public class SubmitHamClientTests
    {
        [Test]
        public void SubmitHamClientTests_Post()
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
            var client = new SubmitHamClient(akismetSettings, httpClient);

            var comment = new Comment(new Uri("http://blog.andrewgunn.co.uk/"), "127.0.0.1", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36");
            comment.CommentType = CommentType.Comment;
            comment.Name = "Andrew Gunn";
            comment.EmailAddress = "hello@andrewgunn.co.uk";
            comment.WebsiteUrl = "http://andrewgunn.co.uk/";
            comment.Body = "This is a test.";
            comment.CommentUri = new Uri("http://blog.andrewgunn.co.uk/post/123/test");
            comment.Referrer = "https://www.google.co.uk/";
            comment.CreatedAt = new DateTime(9999, 12, 31, 23, 59, 59);
            comment.PostModifiedAt = new DateTime(9999, 12, 31, 23, 59, 59);
            comment.Languages = new[] { "en", "en-GB" };
            comment.Encoding = "UTF-8";
            comment.UserRole = "user";
            comment.IsTestMode = true;

            client.PostAsync(comment, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

            requestUri.AbsoluteUri.Should().Be($"https://{akismetApiKey}.rest.akismet.com/1.1/submit-ham");

            requestFormData.ShouldBeEquivalentTo(comment);
        }
    }
}
