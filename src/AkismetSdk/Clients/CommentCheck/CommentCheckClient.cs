namespace AkismetSdk.Clients.CommentCheck
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class CommentCheckClient
    {
        private readonly AkismetSettings _akismetSettings;
        private readonly HttpClient _httpClient;

        public CommentCheckClient(AkismetSettings akismetSettings, HttpClient httpClient)
        {
            _akismetSettings = akismetSettings;
            _httpClient = httpClient;
        }

        public async Task<CommentCheckResult> PostAsync(Comment comment, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_akismetSettings.ApiKey))
            {
                throw new AkismetApiKeyCannotBeNullOrWhitespaceException(_akismetSettings.ApiKey);
            }

            var requestUri = $"https://{_akismetSettings.ApiKey}.rest.akismet.com/1.1/comment-check";
            var formData = new CommentToFormData().Map(comment);
            var requestContent = new FormUrlEncodedContent(formData);

            using (var response = await _httpClient.PostAsync(requestUri, requestContent, cancellationToken))
            {
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var isSpam = responseContent == "true";

                return new CommentCheckResult(isSpam);
            }
        }
    }
}