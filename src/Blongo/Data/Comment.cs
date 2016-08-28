namespace Blongo.Data
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Comment
    {
        public Comment()
        {
            Commenter = new Commenter();
        }

        public string Body { get; set; }

        public Commenter Commenter { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public bool? IsAkismetSpam { get; set; }

        public bool IsSpam { get; set; }

        public ObjectId PostId { get; set; }
    }
}