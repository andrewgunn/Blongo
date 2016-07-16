using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Blongo.Data
{
    public class Comment
    {
        public Comment()
        {
            Commenter = new Commenter();
        }

        public string Body { get; set; }

        public DateTime CreatedAt => Id.CreationTime;

        public Commenter Commenter { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public bool? IsAkismetSpam { get; set; }

        public bool IsSpam { get; set; }

        public ObjectId PostId { get; set; }
    }
}
