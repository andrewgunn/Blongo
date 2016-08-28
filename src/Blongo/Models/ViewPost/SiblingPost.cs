namespace Blongo.Models.ViewPost
{
    using MongoDB.Bson;

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