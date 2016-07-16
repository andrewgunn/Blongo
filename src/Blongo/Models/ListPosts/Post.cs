using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Blongo.Models.ListPosts
{
    public class Post
    {
        public Post(ObjectId id, string title, string body, IReadOnlyCollection<Tag> tags, int commentCount, string urlSlug, DateTime createdAt, DateTime publishedAt)
        {
            Id = id;
            Title = title;
            Body = body;
            Tags = tags ?? new Tag[0];
            CommentCount = commentCount;
            UrlSlug = urlSlug;
            CreatedAt = createdAt;
            PublishedAt = publishedAt;
        }

        public string Body { get; }

        public int CommentCount { get; }

        public DateTime CreatedAt { get; }

        public ObjectId Id { get; }

        public DateTime PublishedAt { get; }

        public IReadOnlyCollection<Tag> Tags { get; }

        public string Title { get; }

        public string UrlSlug { get; }
    }
}
