namespace AkismetSdk
{
    using System;

    public class AkismetApiKeyCannotBeNullOrWhitespaceException : Exception
    {
        public AkismetApiKeyCannotBeNullOrWhitespaceException(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}