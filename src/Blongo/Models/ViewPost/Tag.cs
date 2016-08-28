namespace Blongo.Models.ViewPost
{
    public class Tag
    {
        public Tag(string value, string urlSlug)
        {
            Value = value;
            UrlSlug = urlSlug;
        }

        public string UrlSlug { get; }

        public string Value { get; }
    }
}