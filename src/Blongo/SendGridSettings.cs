namespace Blongo
{
    public class SendGridSettings
    {
        public SendGridSettings(string username, string password, string fromEmailAddress)
        {
            Username = username;
            Password = password;
            FromEmailAddress = fromEmailAddress;
        }

        public string FromEmailAddress { get; }

        public string Password { get; }

        public string Username { get; }
    }
}
