namespace Blongo.Models.FuturePosts
{
    using System;
    using MongoDB.Bson;

    public class Post
    {
        public Post(ObjectId id, string title, DateTime publishedAt)
        {
            Id = id;
            Title = title;
            PublishedAt = publishedAt;
        }

        public ObjectId Id { get; }

        public DateTime PublishedAt { get; }

        public string Title { get; }
    }
}