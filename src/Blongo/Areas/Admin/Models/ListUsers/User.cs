namespace Blongo.Areas.Admin.Models.ListUsers
{
    using MongoDB.Bson;

    public class User
    {
        public User(ObjectId id, string role, string name, string emailAddress)
        {
            Id = id;
            Role = role;
            Name = name;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }

        public ObjectId Id { get; }

        public string Name { get; }

        public string Role { get; }
    }
}