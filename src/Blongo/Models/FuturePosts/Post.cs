using MongoDB.Bson;
using System;

namespace Blongo.Models.FuturePosts
{
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
