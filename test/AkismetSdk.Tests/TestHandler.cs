using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AkismetSdk.Tests
{
    public class TestHandler : DelegatingHandler
    {
        public TestHandler(Func<HttpRequestMessage, HttpResponseMessage> response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response(request));
        }

        private readonly Func<HttpRequestMessage, HttpResponseMessage> _response;
    }
}
