namespace Blongo.Areas.Admin.Models.ListComments
{
    using System;
    using MongoDB.Bson;

    public class Comment
    {
        public Comment(ObjectId id, ObjectId postId, string body, Commenter commenter, bool? isAkismetSpam, bool isSpam,
            DateTime createdAt)
        {
            Id = id;
            PostId = postId;
            Body = body;
            Commenter = commenter;
            IsAkismetSpam = isAkismetSpam;
            IsSpam = isSpam;
            CreatedAt = createdAt;
        }

        public string Body { get; }

        public Commenter Commenter { get; }

        public DateTime CreatedAt { get; }

        public ObjectId Id { get; }

        public bool? IsAkismetSpam { get; }

        public bool IsSpam { get; }

        public ObjectId PostId { get; }
    }
}