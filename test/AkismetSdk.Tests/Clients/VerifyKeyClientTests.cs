namespace AkismetSdk.Tests.Clients
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using AkismetSdk.Clients.VerifyKey;
    using FluentAssertions;
    using NUnit.Framework;

    public class VerifyKeyClientTests
    {
        [Test]
        public void VerifyKeyClient_Post()
        {
            var akismetApiKey = Guid.NewGuid().ToString();

            Uri requestUri = null;
            var httpClient = HttpClientFactory.Create(new TestHandler(r =>
            {
                requestUri = r.RequestUri;

                var responseMessage = new HttpResponseMessage
                {
                    Content = new StringContent("valid")
                };
                responseMessage.StatusCode = HttpStatusCode.OK;

                return responseMessage;
            }));

            var akismetSettings = new AkismetSettings(akismetApiKey);
            var client = new VerifyKeyClient(akismetSettings, httpClient);

            var blogUri = new Uri("http://blog.andrewgunn.co.uk");

            var result =
                client.PostAsync(blogUri, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();

            requestUri.AbsoluteUri.Should().Be("https://rest.akismet.com/1.1/verify-key");

            result.Should().BeTrue();
        }
    }
}