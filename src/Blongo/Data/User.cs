namespace Blongo.Data
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public class User
    {
        public string EmailAddress { get; set; }

        public ObjectId Id { get; set; }

        public string HashedPassword { get; set; }

        public string Name { get; set; }

        public string PasswordSalt { get; set; }

        public IReadOnlyCollection<ResetPasswordLink> ResetPasswordLinks { get; set; }

        public string Role { get; set; }
    }
}