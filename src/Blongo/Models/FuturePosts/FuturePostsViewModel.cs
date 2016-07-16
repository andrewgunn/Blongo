using System.Collections.Generic;

namespace Blongo.Models.FuturePosts
{
    public class FuturePostsViewModel
    {
        public FuturePostsViewModel(IReadOnlyCollection<Post> posts)
        {
            Posts = posts;
        }

        public IReadOnlyCollection<Post> Posts { get; }
    }
}
