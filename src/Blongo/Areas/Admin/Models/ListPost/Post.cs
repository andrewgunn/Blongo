using MongoDB.Bson;
using System;

namespace Blongo.Areas.Admin.Models.ListPosts
{
    public class Post
    {
        public Post(ObjectId id, string title, DateTime publishedAt, bool isPublished)
        {
            Id = id;
            Title = title;
            PublishedAt = publishedAt;
            IsPublished = isPublished;
        }

        public ObjectId Id { get; }

        public bool IsPublished { get; }

        public DateTime PublishedAt { get; }

        public string Title { get; }
    }
}
