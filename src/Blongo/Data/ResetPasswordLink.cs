namespace Blongo.Data
{
    using System;
    using MongoDB.Bson;

    public class ResetPasswordLink
    {
        public ObjectId Id { get; set; }

        public DateTime ExpiresAt { get; set; }

        public ObjectId UserId { get; set; }
    }
}