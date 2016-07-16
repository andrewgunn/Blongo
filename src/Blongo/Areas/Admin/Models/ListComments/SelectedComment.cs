using MongoDB.Bson;

namespace Blongo.Areas.Admin.Models.ListComments
{
    public class SelectedComment
    {
        public SelectedComment(ObjectId id, ObjectId postId, string body, bool isSpam, string commenterWebsiteUrl)
        {
            Id = id;
            PostId = postId;
            Body = body;
            IsSpam = isSpam;
            CommenterWebsiteUrl = commenterWebsiteUrl;
        }

        public string Body { get; }

        public string CommenterWebsiteUrl { get; }

        public ObjectId Id { get; }

        public bool IsSpam { get; }

        public ObjectId PostId { get; }
    }
}
