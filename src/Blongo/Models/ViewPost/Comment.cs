﻿namespace Blongo.Models.ViewPost
{
    using System;
    using MongoDB.Bson;

    public class Comment
    {
        public Comment(ObjectId id, ObjectId postId, string body, Commenter commenter, DateTime createdAt)
        {
            Id = id;
            PostId = postId;
            Body = body;
            Commenter = commenter;
            CreatedAt = createdAt;
        }

        public string Body { get; }

        public DateTime CreatedAt { get; }

        public Commenter Commenter { get; }

        public ObjectId Id { get; }

        public ObjectId PostId { get; }
    }
}