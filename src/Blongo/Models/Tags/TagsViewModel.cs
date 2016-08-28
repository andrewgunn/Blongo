namespace Blongo.Models.Tags
{
    using System.Collections.Generic;

    public class TagsViewModel
    {
        public TagsViewModel(IReadOnlyCollection<Tag> tags)
        {
            Tags = tags;
        }

        public IReadOnlyCollection<Tag> Tags { get; }
    }
}