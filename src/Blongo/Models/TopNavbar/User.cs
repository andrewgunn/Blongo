namespace Blongo.Models.TopNavbar
{
    public class User
    {
        public User(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }

        public string Name { get; }
    }
}