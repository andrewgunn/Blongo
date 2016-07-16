using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blongo.Models.ViewPost
{
    public class SiblingPost
    {
        public SiblingPost(ObjectId id, string title, string urlSlug)
        {
            Id = id;
            Title = title;
            UrlSlug = urlSlug;
        }

        public ObjectId Id { get; }

        public string Title { get; }

        public string UrlSlug { get; }
    }
}
