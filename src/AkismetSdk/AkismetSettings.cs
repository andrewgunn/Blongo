namespace AkismetSdk
{
    public class AkismetSettings
    {
        public AkismetSettings(string apiKey)
        {
            ApiKey = apiKey;
        }

        public string ApiKey { get; }
    }
}
