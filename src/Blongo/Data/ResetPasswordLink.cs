using MongoDB.Bson;
using System;

namespace Blongo.Data
{
    public class ResetPasswordLink
    {
        public ObjectId Id { get; set; }

        public DateTime ExpiresAt { get; set; }
        
        public ObjectId UserId { get; set; } 
    }
}
