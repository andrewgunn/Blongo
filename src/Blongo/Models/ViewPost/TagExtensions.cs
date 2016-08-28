namespace Blongo.Models.ViewPost
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TagExtensions
    {
        public static IReadOnlyCollection<Tag> ToTagViewModels(this IReadOnlyCollection<Data.Tag> tags)
        {
            return tags.Select(t => new Tag(t.Value, t.UrlSlug)).ToList();
        }
    }
}