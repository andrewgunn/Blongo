using System.Collections.Generic;
using System.Linq;

namespace Blongo.Models.ListPosts
{
    public static class TagExtensions
    {
        public static IReadOnlyCollection<Tag> ToTagViewModels(this IReadOnlyCollection<Data.Tag> tags)
        {
            return tags.Select(t => new Tag(t.Value, t.UrlSlug)).ToList();
        }
    }
}
