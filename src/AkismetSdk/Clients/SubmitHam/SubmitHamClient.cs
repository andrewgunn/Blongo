using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AkismetSdk.Clients.SubmitHam
{
    public class SubmitHamClient
    {
        public SubmitHamClient(AkismetSettings akismetSettings, HttpClient httpClient)
        {
            _akismetSettings = akismetSettings;
            _httpClient = httpClient;
        }

        public async Task PostAsync(Comment comment, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_akismetSettings.ApiKey))
            {
                throw new AkismetApiKeyCannotBeNullOrWhitespaceException(_akismetSettings.ApiKey);
            }

            var requestUri = $"https://{_akismetSettings.ApiKey}.rest.akismet.com/1.1/submit-ham";
            var formData = new CommentToFormData().Map(comment);
            var requestContent = new FormUrlEncodedContent(formData);

            using (var response = await _httpClient.PostAsync(requestUri, requestContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        private readonly AkismetSettings _akismetSettings;
        private readonly HttpClient _httpClient;
    }
}
