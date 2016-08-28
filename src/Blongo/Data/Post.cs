namespace Blongo.Data
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Post
    {
        public Post()
        {
            Tags = new List<Tag>();
        }

        public string Body { get; set; }

        public int CommentCount { get; set; }

        public string Description { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        public DateTime PublishedAt { get; set; }

        public string Scripts { get; set; }

        public string Styles { get; set; }

        public List<Tag> Tags { get; set; }

        public string Title { get; set; }

        public string UrlSlug { get; set; }
    }
}