using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace AkismetSdk.Clients.VerifyKey
{
    public class VerifyKeyClient
    {
        public VerifyKeyClient(AkismetSettings akismetSettings, HttpClient httpClient)
        {
            _akismetSettings = akismetSettings;
            _httpClient = httpClient;
        }

        public async Task<bool> PostAsync(Uri blogUri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_akismetSettings.ApiKey))
            {
                throw new AkismetApiKeyCannotBeNullOrWhitespaceException(_akismetSettings.ApiKey);
            }

            var requestUri = "https://rest.akismet.com/1.1/verify-key";
            var formData = new[]
            {
                new KeyValuePair<string, string>("key", _akismetSettings.ApiKey),
                new KeyValuePair<string, string>("blog", blogUri.AbsoluteUri)
            };
            var requestContent = new FormUrlEncodedContent(formData);

            using (var responseMessage = await _httpClient.PostAsync(requestUri, requestContent, cancellationToken))
            {
                responseMessage.EnsureSuccessStatusCode();

                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                return responseContent == "valid";
            }
        }

        private readonly AkismetSettings _akismetSettings;
        private readonly HttpClient _httpClient;
    }
}
