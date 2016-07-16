using MongoDB.Bson;

namespace Blongo.Areas.Admin.Models.ListPosts
{
    public class SelectedPost
    {
        public SelectedPost(ObjectId id, string body, bool isPublished)
        {
            Id = id;
            Body = body;
            IsPublished = isPublished;
        }

        public string Body { get; }

        public ObjectId Id { get; }

        public bool IsPublished { get; }
    }
}
