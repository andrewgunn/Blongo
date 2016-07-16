namespace Blongo.Models.Tags
{
    public class Tag
    {
        public Tag(string value, string urlSlug, int count)
        {
            Value = value;
            UrlSlug = urlSlug;
            Count = count;
        }

        public int Count { get; }

        public string UrlSlug { get; }

        public string Value { get; }
    }
}
