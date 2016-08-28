namespace Blongo.Models.FuturePosts
{
    using System.Collections.Generic;

    public class FuturePostsViewModel
    {
        public FuturePostsViewModel(IReadOnlyCollection<Post> posts)
        {
            Posts = posts;
        }

        public IReadOnlyCollection<Post> Posts { get; }
    }
}