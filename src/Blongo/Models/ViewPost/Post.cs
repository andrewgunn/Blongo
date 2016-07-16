using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Blongo.Models.ViewPost
{
    public class Post
    {
        public Post(ObjectId id, string title, string description, string body, IReadOnlyCollection<Tag> tags, int commentCount, DateTime publishedAt, string urlSlug, DateTime createdAt, bool isPublished)
        {
            Id = id;
            Title = title;
            Description = description;
            Body = body;
            Tags = tags ?? new Tag[0];
            CommentCount = commentCount;
            UrlSlug = urlSlug;
            CreatedAt = createdAt;
            PublishedAt = publishedAt;
            IsPublished = isPublished;
        }

        public string Body { get; }

        public int CommentCount { get; }

        public DateTime CreatedAt { get; }

        public string Description { get; }

        public ObjectId Id { get; }

        public bool IsPublished { get; }

        public DateTime PublishedAt { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public string Title { get; }

        public string UrlSlug { get; }
    }
}
