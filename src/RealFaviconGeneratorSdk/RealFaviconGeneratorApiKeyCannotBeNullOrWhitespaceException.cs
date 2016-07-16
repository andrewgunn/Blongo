using System;

namespace RealFaviconGeneratorSdk
{
    public class RealFaviconGeneratorApiKeyCannotBeNullOrWhitespaceException : Exception
    {
        public RealFaviconGeneratorApiKeyCannotBeNullOrWhitespaceException(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}
