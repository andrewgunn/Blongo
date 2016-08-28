namespace RealFaviconGeneratorSdk
{
    using System;

    public class RealFaviconGeneratorApiKeyCannotBeNullOrWhitespaceException : Exception
    {
        public RealFaviconGeneratorApiKeyCannotBeNullOrWhitespaceException(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}