using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blongo.Data
{
    public class Blog
    {
        public string AkismetApiKey { get; set; }

        public Author Author { get; set; }

        public string AzureStorageConnectionString { get; set; }

        public Company Company { get; set; }

        public string Description { get; set; }

        public string FaviconsHtml { get; set; }

        public string FeedUrl { get; set; }

        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string RealFaviconGeneratorApiKey { get; set; }

        public string Scripts { get; set; }

        public SendGridSettings SendGridSettings { get; set; }

        public string Styles { get; set; }
    }
}
