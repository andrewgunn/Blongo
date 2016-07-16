using System.Collections.Generic;

namespace Blongo.Models.Tags
{
    public class TagsViewModel
    {
        public TagsViewModel(IReadOnlyCollection<Tag> tags)
        {
            Tags = tags;
        }

        public IReadOnlyCollection<Tag> Tags { get; }
    }
}
