using System;

namespace AkismetSdk
{
    public class AkismetApiKeyCannotBeNullOrWhitespaceException : Exception
    {
        public AkismetApiKeyCannotBeNullOrWhitespaceException(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}
